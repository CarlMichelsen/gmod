using Application.Mapper;
using Database.Entity.Id;
using Microsoft.Extensions.Logging;
using Presentation.Dto;
using Presentation.Dto.Image;
using Presentation.Handler;
using Presentation.Service;
using Presentation.Service.Exception;

namespace Application.Handler;

public class ImageHandler(
    ILogger<ImageHandler> logger,
    IImageService imageService) : IImageHandler
{
    public async Task<ServiceResponse<ImageResponseDto>> LoadImage(ImageRequestDto imageRequest)
    {
        try
        {
            var imageEntity = await imageService.Load(
                imageRequest.Source,
                imageRequest.ImageName,
                imageRequest.SquareResolution,
                imageRequest.CreatorSteamId64);
            
            logger.LogInformation(
                "'{CreatorSteamId}' Loaded image '{ImageName}' from '{ImageSource}'",
                imageRequest.CreatorSteamId64,
                imageRequest.ImageName,
                imageRequest.Source);
            
            return new ServiceResponse<ImageResponseDto>(imageEntity.ToResponseDto());
        }
        catch (GmodException e)
        {
            return new ServiceResponse<ImageResponseDto>(e.Message);
        }
    }

    public async Task<ServiceResponse<ImageResponseDto>> GetImage(Guid imageId)
    {
        var noContentImageEntity = await imageService
            .GetWithoutContent(new ImageEntityId(imageId));

        if (noContentImageEntity is null)
        {
            return new ServiceResponse<ImageResponseDto>("Image not found");
        }

        return new ServiceResponse<ImageResponseDto>(noContentImageEntity.ToResponseDto());
    }

    public async Task<ServiceResponse<List<ImageResponseDto>>> GetImages(int skip, int take)
    {
        var noContentImages = await imageService.GetManyWithoutContent(skip, take);
        var imageDtoList = noContentImages
            .Select(x => x.ToResponseDto())
            .ToList();

        return new ServiceResponse<List<ImageResponseDto>>(imageDtoList);
    }

    public async Task<string?> GetPartialImage(Guid imageId, int skip, int take)
    {
        try
        {
            return await imageService
                .GetPartialImageData(new ImageEntityId(imageId), skip, take);
        }
        catch (GmodException e)
        {
            logger.LogWarning(e, "Did not find any data");
            return null;
        }
    }

    public async Task<ServiceResponse<ImageResponseDto>> DeleteImage(Guid imageId)
    {
        var deletedImageIfSuccessful = await imageService
            .Delete(new ImageEntityId(imageId));

        return deletedImageIfSuccessful is null
            ? new ServiceResponse<ImageResponseDto>("Image not found")
            : new ServiceResponse<ImageResponseDto>(deletedImageIfSuccessful.ToResponseDto());
    }
}