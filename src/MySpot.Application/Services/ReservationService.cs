using MySpot.Application.Time;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationService : IReservationService
{

    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _repository;

    public ReservationService(IClock clock, IWeeklyParkingSpotRepository repository)
    {
        _clock = clock;
        _repository = repository;
    }


    public ReservationDto? FindById(Guid id) => FindAllWeekly().SingleOrDefault(reservation => reservation.Id == id);

    public IEnumerable<ReservationDto> FindAllWeekly() => _repository.FindAll().SelectMany(spot => spot.Reservations).Select(reservation => new ReservationDto
    {
        Id = reservation.Id,
        ParkingSpotId = reservation.ParkingSpotId,
        Date = reservation.Date,
        LicensePlate = reservation.LicensePlate
    });

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _repository.FindById(command.ParkingSpotId);

        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName, command.LicensePlate, command.Date);
        Console.WriteLine(command.Date.ToString());
        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
        _repository.Update(weeklyParkingSpot);
        return reservation.Id;
    }


    public bool Update(UpdateReservationLicensePlate command)
    {
        var weeklyParkingSpot = FindWeeklySpotByReservation(command.id);

        if (weeklyParkingSpot == null)
        {
            return false;
        }

        var reservartionToUpdate = weeklyParkingSpot.Reservations.SingleOrDefault(reservation => reservation.Id == new ReservationId(command.id));



        if (reservartionToUpdate == null || reservartionToUpdate.Date <= new Date(_clock.Current()))
        {
            return false;
        }

        reservartionToUpdate.ChangeLicensePlate(command.LicensePlate);
        _repository.Update(weeklyParkingSpot);
        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var parkingSpot = FindWeeklySpotByReservation(command.id);

        if (parkingSpot is null)
        {
            return false;
        }
        var reservation = parkingSpot.Reservations.SingleOrDefault(reservation => reservation.Id == new ReservationId(command.id));
        if (reservation == null)
        {
            return false;
        }
        parkingSpot.RemoveReservation(reservation);
        _repository.Update(parkingSpot);

        return true;
    }


    private WeeklyParkingSpot? FindWeeklySpotByReservation(ReservationId reservationId) => _repository.FindAll().SingleOrDefault(spot => spot.Reservations.Any(reservation => reservation.Id == reservationId));
}
