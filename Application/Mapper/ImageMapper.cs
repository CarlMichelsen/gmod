using Database.Entity;
using Presentation.Dto.Image;

namespace Application.Mapper;

public static class ImageMapper
{
    public static ImageResponseDto ToResponseDto(this ImageEntity imageEntity)
        => new ImageResponseDto(
            ImageId: imageEntity.Id.Value,
            Name: imageEntity.Name,
            Size: new SizeDto(imageEntity.SizeX, imageEntity.SizeY),
            CreatorSteamId64: imageEntity.CreatorSteamId64.ToString(),
            ContentHashCode: imageEntity.ContentHashCode.ToString(),
            Source: imageEntity.Source);
}