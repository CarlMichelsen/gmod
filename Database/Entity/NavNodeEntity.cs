using System.ComponentModel.DataAnnotations;
using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class NavNodeEntity : IEntity
{
    public required NavNodeEntityId Id { get; init; }
    
    public required List<NavNodeEntity> LinkedTo { get; init; }
    
    public required List<NavNodeEntity> LinkedFrom { get; init; }
    
    [MaxLength(34)]
    public required string Tag { get; init; }
    
    [MaxLength(128)]
    public required string Map { get; init; }
    
    public required int X { get; init; }
    
    public required int Y { get; init; }
    
    public required int Z { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NavNodeEntity>();
        
        entity
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<NavNodeEntity, NavNodeEntityId>(guid =>
                new NavNodeEntityId(guid, true));

        entity.HasKey(x => x.Id);
        
        entity.HasMany(n => n.LinkedTo)
            .WithMany(n => n.LinkedFrom)
            .UsingEntity<NavNodeLinkEntity>(
                l => l
                    .HasOne(link => link.To)
                    .WithMany()
                    .HasForeignKey(link => link.ToId)
                    .OnDelete(DeleteBehavior.Cascade),
                r => r
                    .HasOne(link => link.From)
                    .WithMany()
                    .HasForeignKey(link => link.FromId)
                    .OnDelete(DeleteBehavior.Cascade));

        entity.HasIndex(x => x.Map);
        
        entity.HasIndex(x => x.Tag);
    }
}