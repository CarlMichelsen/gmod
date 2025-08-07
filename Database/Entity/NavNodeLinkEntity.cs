using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class NavNodeLinkEntity : IEntity
{
    public required NavNodeEntityId ToId { get; init; }
    
    public required NavNodeEntityId FromId { get; init; }
    
    public NavNodeEntity? To { get; init; }
    
    public NavNodeEntity? From { get; init; }
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NavNodeLinkEntity>();
        
        entity
            .Property(x => x.ToId)
            .RegisterTypedKeyConversion<NavNodeEntity, NavNodeEntityId>(guid =>
                new NavNodeEntityId(guid, true));
        
        entity
            .Property(x => x.FromId)
            .RegisterTypedKeyConversion<NavNodeEntity, NavNodeEntityId>(guid =>
                new NavNodeEntityId(guid, true));
        
        entity.HasKey(l => new { l.ToId, l.FromId });
        
        // Configure the relationships to the principal entities
        entity.HasOne(l => l.To)
            .WithMany()
            .HasForeignKey(l => l.ToId)
            .OnDelete(DeleteBehavior.Cascade);
            
        entity.HasOne(l => l.From)
            .WithMany()
            .HasForeignKey(l => l.FromId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}