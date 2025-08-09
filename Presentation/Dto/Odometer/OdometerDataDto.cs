namespace Presentation.Dto.Odometer;

public record OdometerDataDto(
    int TotalUnitsTravelled,
    string Map,
    string Tag);