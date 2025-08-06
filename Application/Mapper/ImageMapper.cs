using Database.Entity;
using Presentation.Dto.Image;

namespace Application.Mapper;

public static class ImageMapper
{
    public static ImageResponseDto ToResponseDto(this ImageEntity imageEntity)
        => new ImageResponseDto(
            ImageId: imageEntity.Id.Value,
            Size: new SizeDto(imageEntity.SizeX, imageEntity.SizeY),
            CreatorSteamId64: imageEntity.CreatorSteamId64,
            ContentHashCode: imageEntity.ContentHashCode,
            Source: imageEntity.Source);
}