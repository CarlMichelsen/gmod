using System.ComponentModel.DataAnnotations;
using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class OdometerTripEntity : IEntity
{
    public required OdometerTripEntityId Id { get; init; }
    
    public required DateTime StartedAt { get; init; }
    
    [MaxLength(256)]
    public required string Map { get; init; }
    
    [MaxLength(256)]
    public required string Tag { get; init; }
    
    public required List<OdometerDataEntity> OdometerData { get; init; }
    
    public DateTime? EndedAt { get; init; }
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<OdometerTripEntity>();
        
        entity
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<OdometerTripEntity, OdometerTripEntityId>(guid =>
                new OdometerTripEntityId(guid, true));
        
        entity.HasKey(x => x.Id);
        
        entity
            .HasMany(x => x.OdometerData)
            .WithOne(x => x.ParentOdometerTrip)
            .HasForeignKey(x => x.ParentOdometerTripId);
    }
}