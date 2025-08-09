using Database;
using Database.Entity;
using Database.Entity.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentation.Service;
using Presentation.Service.Exception;

namespace Application.Service;

public class OdometerService(
    ILogger<OdometerService> logger,
    TimeProvider timeProvider,
    ApplicationContext applicationContext) : IOdometerService
{
    public async Task<(OdometerTripEntityId Id, IOdometerService.OdometerData Data)> StartNewTrip(
        string map,
        string tag)
    {
        var tripData = await this.GetOdometerData(map, tag);
        var trip = new OdometerTripEntity
        {
            Id = new OdometerTripEntityId(Guid.CreateVersion7()),
            StartedAt = timeProvider.GetLocalNow().UtcDateTime.ToUniversalTime(),
            Map = map,
            Tag = tag,
            OdometerData = [],
        };
        
        applicationContext.OdometerTrip.Add(trip);
        await applicationContext.SaveChangesAsync();
        logger.LogInformation(
            "Started new trip <{TripId}>",
            trip.Id);
        
        return (trip.Id, tripData);
    }

    public async Task AppendTrip(
        string map,
        string tag,
        OdometerTripEntityId tripId,
        List<OdometerDataEntity.Position> data)
    {
        var existingTrip = await applicationContext
            .OdometerTrip
            .FirstOrDefaultAsync(t =>
                t.Map == map &&
                t.Tag == tag &&
                t.Id == tripId);
        if (existingTrip is null)
        {
            throw new GmodException("Did not find existing trip");
        }

        var dataEntity = new OdometerDataEntity
        {
             Id = new OdometerDataEntityId(Guid.CreateVersion7()),
             ParentOdometerTripId = existingTrip.Id,
             ParentOdometerTrip = existingTrip,
             Positions = data,
             ReceivedAt = timeProvider.GetLocalNow().UtcDateTime.ToUniversalTime(),
        };
        existingTrip.OdometerData.Add(dataEntity);
        
        await applicationContext.SaveChangesAsync();
        logger.LogInformation(
            "Appended trip <{TripId}> with Chunk <{ChunkId}> which has {DataPointAmount} data points",
            existingTrip.Id,
            dataEntity.Id,
            dataEntity.Positions.Count);
    }

    public async Task<IOdometerService.OdometerData> GetOdometerData(
        string map,
        string tag)
    {
        var trips = await applicationContext
            .OdometerTrip
            .Where(t =>
                t.Map == map &&
                t.Tag == tag)
            .OrderByDescending(t => t.StartedAt)
            .Include(t => t.OdometerData)
            .ToListAsync();
        
        var waypoints = trips
            .SelectMany(t => t.OdometerData)
            .OrderByDescending(o => o.ReceivedAt)
            .SelectMany(o => o.Positions)
            .ToList();
        
        var totalDistance = waypoints
            .Zip(waypoints.Skip(1), (p1, p2) =>
                Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2)))
            .Sum();

        return new IOdometerService.OdometerData(
            TotalUnitsTravelled: (int)totalDistance,
            Map: map,
            Tag: tag);
    }
}