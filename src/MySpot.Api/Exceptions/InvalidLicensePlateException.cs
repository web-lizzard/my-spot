
namespace MySpot.Api.Exceptions;
public sealed class InvalidLicensePlateExcepton : CustomException
{
    public InvalidLicensePlateExcepton(string licensePlate) : base($"License plate: {licensePlate} is invalid")
    {
    }
}