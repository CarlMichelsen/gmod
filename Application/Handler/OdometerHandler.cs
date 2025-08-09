using Database.Entity;
using Database.Entity.Id;
using Presentation.Dto;
using Presentation.Dto.Odometer;
using Presentation.Handler;
using Presentation.Service;
using Presentation.Service.Exception;

namespace Application.Handler;

public class OdometerHandler(
    IOdometerService odometerService) : IOdometerHandler
{
    public async Task<ServiceResponse<NewTripDto>> StartNewTrip(
        string map,
        string tag)
    {
        try
        {
            var newTripTuple = await odometerService
                .StartNewTrip(map, tag);

            var tripDto = new NewTripDto(
                TripId: newTripTuple.Id,
                TotalUnitsTravelled: newTripTuple.Data.TotalUnitsTravelled,
                Map: newTripTuple.Data.Map,
                Tag: newTripTuple.Data.Tag);
            return new ServiceResponse<NewTripDto>(tripDto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<NewTripDto>(e.Message);
        }
    }

    public async Task<ServiceResponse> AppendTrip(
        string map,
        string tag,
        Guid tripId,
        OdometerChunkDto chunk)
    {
        try
        {
            var position = chunk.Positions
                .Select(p => new OdometerDataEntity.Position(p.X, p.Y, p.Z))
                .ToList();
            await odometerService.AppendTrip(
                map,
                tag,
                new OdometerTripEntityId(tripId),
                position);

            return new ServiceResponse<NewTripDto>();
        }
        catch (GmodException e)
        {
            return new ServiceResponse<NewTripDto>(e.Message);
        }
    }

    public async Task<ServiceResponse<OdometerDataDto>> GetOdometerData(
        string map,
        string tag)
    {
        try
        {
            var odometerData = await odometerService
                .GetOdometerData(map, tag);
            
            return new ServiceResponse<OdometerDataDto>(new OdometerDataDto(
                TotalUnitsTravelled: odometerData.TotalUnitsTravelled,
                Map: odometerData.Map,
                Tag: odometerData.Tag));
        }
        catch (GmodException e)
        {
            return new ServiceResponse<OdometerDataDto>(e.Message);
        }
    }
}