namespace Presentation.Dto.Odometer;

public record NewTripDto(
    Guid TripId,
    int TotalUnitsTravelled,
    string Map,
    string Tag) : OdometerDataDto(TotalUnitsTravelled, Map, Tag);