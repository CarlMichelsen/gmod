using System.ComponentModel.DataAnnotations;
using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class ImageEntity : IEntity
{
    public required ImageEntityId Id { get; init; }
    
    public required ImageContentEntityId ContentId { get; init; }
    
    [MaxLength(100)]
    public required string Name { get; init; }
    
    public required int SizeX { get; init; }
    
    public required int SizeY { get; init; }
    
    public required ulong? CreatorSteamId64 { get; init; }
    
    public required Uri Source { get; init; }
    
    public required int ContentHashCode { get; init; }
    
    public ImageContentEntity? Content { get; init; }

    public required DateTime CreatedAt { get; init; }
    
    public int SquareResolution => (int)Math.Round((double)Math.Max(this.SizeX, this.SizeY));
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ImageEntity>();
        
        entity
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<ImageEntity, ImageEntityId>(guid =>
                new ImageEntityId(guid, true));
        
        entity
            .Property(x => x.ContentId)
            .RegisterTypedKeyConversion<ImageContentEntity, ImageContentEntityId>(guid =>
                new ImageContentEntityId(guid, true));

        entity
            .HasOne(x => x.Content)
            .WithOne(x => x.Image)
            .HasForeignKey<ImageEntity>(x => x.ContentId);

        entity
            .HasIndex(x => x.ContentHashCode);
    }
}