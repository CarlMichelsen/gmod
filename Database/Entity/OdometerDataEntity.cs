using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class OdometerDataEntity : IEntity
{
    public const char PositionDelimiter = ';';
    
    public required OdometerDataEntityId Id { get; init; }
    
    public required OdometerTripEntityId ParentOdometerTripId { get; init; }
    
    public OdometerTripEntity? ParentOdometerTrip { get; init; }

    public required List<Position> Positions { get; init; }
    
    public required DateTime ReceivedAt { get; init; }
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<OdometerDataEntity>();
        
        entity
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<OdometerDataEntity, OdometerDataEntityId>(guid =>
                new OdometerDataEntityId(guid, true));
        
        entity.HasKey(x => x.Id);
        
        entity
            .Property(x => x.ParentOdometerTripId)
            .RegisterTypedKeyConversion<OdometerTripEntity, OdometerTripEntityId>(guid =>
                new OdometerTripEntityId(guid, true));

        entity
            .Property(x => x.Positions)
            .HasConversion(
                x => string.Join(PositionDelimiter, x.Select(y => y.ToString())),
                x => x.Split(PositionDelimiter, StringSplitOptions.RemoveEmptyEntries).Select(y => Position.Parse(y)).ToList());

        entity
            .HasOne(x => x.ParentOdometerTrip)
            .WithMany(x => x.OdometerData)
            .HasForeignKey(x => x.ParentOdometerTripId);
    }

    public readonly record struct Position(int X, int Y, int Z)
    {
        public override string ToString()
        {
            return $"{this.X},{this.Y},{this.Z}";
        }

        public static Position Parse(ReadOnlySpan<char> stringPosition)
        {
            Span<Range> ranges = stackalloc Range[3];
            var count = stringPosition.Split(ranges, ',');
    
            if (count != 3)
            {
                throw new ArgumentException("Invalid format", nameof(stringPosition));
            }

            return new Position(
                int.Parse(stringPosition[ranges[0]], CultureInfo.InvariantCulture),
                int.Parse(stringPosition[ranges[1]], CultureInfo.InvariantCulture),
                int.Parse(stringPosition[ranges[2]], CultureInfo.InvariantCulture));
        }
    }
}