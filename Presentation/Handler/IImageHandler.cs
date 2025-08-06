using Presentation.Dto;
using Presentation.Dto.Image;

namespace Presentation.Handler;

public interface IImageHandler
{
    Task<ServiceResponse<ImageResponseDto>> LoadImage(ImageRequestDto imageRequest);

    Task<ServiceResponse<ImageResponseDto>> GetImage(Guid imageId);
    
    Task<ServiceResponse<List<ImageResponseDto>>> GetImages(int skip, int take);

    Task<string?> GetPartialImage(Guid imageId, int skip, int take);
    
    Task<ServiceResponse<ImageResponseDto>> DeleteImage(Guid imageId);
}