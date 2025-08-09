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
    private const int MaximumAllowedDistanceBetweenDataPoints = 500;
    
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
            .Include(t => t.OdometerData)
            .FirstOrDefaultAsync(t =>
                t.Map == map &&
                t.Tag == tag &&
                t.Id == tripId);
        if (existingTrip is null)
        {
            throw new GmodException("Did not find existing trip");
        }

        this.ValidateAppendedDataHasAppropriateResolution(existingTrip, data);

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
            .OrderBy(t => t.StartedAt)
            .Include(t => t.OdometerData)
            .ToListAsync();
        
        return new IOdometerService.OdometerData(
            TotalUnitsTravelled: (int)trips.Select(TotalUnitsTravelled).Sum(),
            Map: map,
            Tag: tag);
    }
    
    private static double TotalUnitsTravelled(OdometerTripEntity tripEntity)
    {
        if (tripEntity.OdometerData is null)
        {
            throw new GmodException(
                "OdometerTrip does not have position data included from the database");
        }
        
        var waypoints = tripEntity.OdometerData
            .SelectMany(o => o.Positions)
            .ToList();
        
        return waypoints
            .Zip(waypoints.Skip(1), (p1, p2) => p1.DistanceTo(p2))
            .Sum();
    }
    
    private void ValidateAppendedDataHasAppropriateResolution(OdometerTripEntity tripEntity, List<OdometerDataEntity.Position> newPositions)
    {
        var waypoints = tripEntity.OdometerData
            .SelectMany(o => o.Positions)
            .ToList();

        var stitchedWaypoints = waypoints.Count > 0
            ? new List<OdometerDataEntity.Position>([waypoints.Last(), ..newPositions])
            : new List<OdometerDataEntity.Position>(newPositions);

        var hasTooLongDistance = stitchedWaypoints
            .Zip(stitchedWaypoints.Skip(1), (p1, p2) => p1.DistanceTo(p2))
            .Any(dist => dist > MaximumAllowedDistanceBetweenDataPoints);

        if (hasTooLongDistance)
        {
            logger.LogWarning("Attempted to append odometer data that had too low resolution");
            throw new GmodException("The distance between appended points is too large");
        }
    }
}