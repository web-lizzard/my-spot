using MySpot.Api.ValueObjects;

namespace MySpot.Api.DTO;

public class ReservationDto
{
    public ReservationDto()
    {
    }

    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public Guid ParkingSpotId { get; set; }

    public string LicensePlate { get; set; }
}