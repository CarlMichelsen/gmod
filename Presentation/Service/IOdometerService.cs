using Database.Entity;
using Database.Entity.Id;

namespace Presentation.Service;

public interface IOdometerService
{
    Task<(OdometerTripEntityId Id, OdometerData Data)> StartNewTrip(string map, string tag);
    
    Task AppendTrip(string map, string tag, OdometerTripEntityId tripId, List<OdometerDataEntity.Position> data);
    
    Task<OdometerData> GetOdometerData(string map, string tag);
    
    public record OdometerData(
        int TotalUnitsTravelled,
        string Map,
        string Tag);
}