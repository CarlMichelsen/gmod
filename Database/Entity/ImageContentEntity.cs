using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class ImageContentEntity : IEntity
{
    public required ImageContentEntityId Id { get; init; }
    
    public required byte[] Data { get; init; }
    
    public required ImageEntityId ImageId { get; init; }
    
    public ImageEntity? Image { get; init; }

    public static void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ImageContentEntity>();
        
        entity
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<ImageContentEntity, ImageContentEntityId>(guid =>
                new ImageContentEntityId(guid, true));
        
        entity
            .Property(x => x.ImageId)
            .RegisterTypedKeyConversion<ImageEntity, ImageEntityId>(guid =>
                new ImageEntityId(guid, true));

        entity
            .HasOne(x => x.Image)
            .WithOne(x => x.Content)
            .HasForeignKey<ImageContentEntity>(x => x.ImageId);
    }
}