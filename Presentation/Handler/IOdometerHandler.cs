using Presentation.Dto;
using Presentation.Dto.Odometer;

namespace Presentation.Handler;

public interface IOdometerHandler
{
    Task<ServiceResponse<NewTripDto>> StartNewTrip(
        string map,
        string tag);
    
    Task<ServiceResponse> AppendTrip(
        string map,
        string tag,
        Guid tripId,
        OdometerChunkDto chunk);
    
    Task<ServiceResponse<OdometerDataDto>> GetOdometerData(
        string map,
        string tag);
}