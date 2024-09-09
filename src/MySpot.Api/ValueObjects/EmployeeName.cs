using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;
public record EmployeeName(string Name)
{

    public string Value { get; } = Name ?? throw new InvalidEmployeeName(Name);

    public static implicit operator string(EmployeeName name) => name.Value;
    public static implicit operator EmployeeName(string value) => new(value);


}