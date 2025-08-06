using System.Text;
using Database;
using Database.Entity;
using Database.Entity.Id;
using Microsoft.EntityFrameworkCore;
using Presentation.Service;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Application.Service;

public class ImageService(
    HttpClient httpClient,
    TimeProvider timeProvider,
    ApplicationContext applicationContext) : IImageService
{
    public async Task<ImageEntity> Load(Uri uri, string imageName, int setResolution, ulong? creatorSteamId64 = null)
    {
        var response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            throw new GmodException("Failed to load image");
        }

        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        var imageResult = GetResizedImagePixels(imageBytes, setResolution);
        var imageDataString = string.Join(',', imageResult.Pixels.Select(p => p.ToString()));
        
        var contentHashCode = imageDataString.GetHashCode();
        var existingImage =
            await applicationContext.Image
                .Include(x => x.Content)
                .FirstOrDefaultAsync(x => x.ContentHashCode == contentHashCode);
        if (existingImage is not null)
        {
            return existingImage;
        }

        var imageId = new ImageEntityId(Guid.CreateVersion7());
        var imageContentId = new ImageContentEntityId(Guid.CreateVersion7());

        var imageContent = new ImageContentEntity
        {
            Id = imageContentId,
            ImageId = imageId,
            Data = Encoding.UTF8.GetBytes(imageDataString),
        };

        var image = new ImageEntity
        {
            Id = imageId,
            ContentId = imageContentId,
            Name = imageName,
            CreatorSteamId64 = creatorSteamId64,
            SizeX = imageResult.SizeX,
            SizeY = imageResult.SizeY,
            Source = uri,
            ContentHashCode = contentHashCode,
            Content = imageContent,
            CreatedAt = timeProvider.GetLocalNow().DateTime.ToUniversalTime(),
        };
        
        applicationContext.Image.Add(image);
        await applicationContext.SaveChangesAsync();

        return image;
    }

    public Task<ImageEntity?> GetWithoutContent(ImageEntityId imageId)
    {
        return applicationContext.Image
            .FirstOrDefaultAsync(x => x.Id == imageId);
    }

    public Task<List<ImageEntity>> GetManyWithoutContent(int skip, int take)
    {
        return applicationContext.Image
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public Task<ImageEntity?> Get(ImageEntityId imageId)
    {
        return applicationContext.Image
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == imageId);
    }

    public async Task<string> GetPartialImageData(ImageEntityId imageId, int skip, int take)
    {
        var fullImage = await this.Get(imageId);
        if (fullImage?.Content is null)
        {
            throw new GmodException("Failed to load image");
        }

        var pixels = Encoding
            .UTF8
            .GetString(fullImage.Content.Data)
            .Split(',');

        var partialPixels = pixels
            .Skip(skip)
            .Take(take);
        
        return string.Join(',', partialPixels);
    }

    public async Task<ImageEntity?> Delete(ImageEntityId imageId)
    {
        var imageToDelete = await applicationContext.Image
            .FirstOrDefaultAsync(x => x.Id == imageId);
        if (imageToDelete is null)
        {
            return null;
        }
        
        applicationContext.Image.Remove(imageToDelete);
        await applicationContext.SaveChangesAsync();
        return imageToDelete;
    }

    private static (List<ImagePixel> Pixels, int SizeX, int SizeY) GetResizedImagePixels(byte[] imageBytes, int setResolution)
    {
        using var image = Image.Load<Rgb24>(imageBytes);
        var scale = Math.Min((float)setResolution / image.Width, (float)setResolution / image.Height);
        var newWidth = (int)(image.Width * scale);
        var newHeight = (int)(image.Height * scale);
        
        image.Mutate(x => x.Resize(newWidth, newHeight));
        var pixels = new List<ImagePixel>();
        
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var pixelRow = accessor.GetRowSpan(y);
                for (var x = 0; x < pixelRow.Length; x++)
                {
                    ref var pixel = ref pixelRow[x];
                    pixels.Add(new ImagePixel(pixel.R, pixel.G, pixel.B));
                }
            }
        });

        return (Pixels: pixels, SizeX: newWidth, SizeY: newHeight);
    }
}