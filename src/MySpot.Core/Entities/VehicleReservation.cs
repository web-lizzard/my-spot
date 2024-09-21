using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;
public sealed class VehicleReservation : Reservation
{
    public VehicleReservation(Guid id, Guid parkingSpotId, EmployeeName employeeName, LicensePlate licensePlate, Date date) : base(id, parkingSpotId, date)
    {
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
    }

    private VehicleReservation() { }
    public EmployeeName EmployeeName { get; private set; }

    public LicensePlate LicensePlate { get; private set; }


    public void ChangeLicensePlate(LicensePlate licensePlate)
    {
        LicensePlate = licensePlate;
    }
}