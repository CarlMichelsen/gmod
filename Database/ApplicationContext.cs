using Database.Entity;

namespace Database;

using Microsoft.EntityFrameworkCore;

public class ApplicationContext(
    DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public const string SchemaName = "gmod";
    
    public DbSet<ImageEntity> Image { get; init; }
    
    public DbSet<ImageContentEntity> ImageContent { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        
        ImageEntity.Configure(modelBuilder);
        ImageContentEntity.Configure(modelBuilder);
    }
}