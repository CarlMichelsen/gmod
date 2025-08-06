using Database.Entity;
using Database.Entity.Id;

namespace Presentation.Service;

public interface IImageService
{
    Task<ImageEntity> Load(Uri uri, string imageName, int setResolution, ulong? creatorSteamId64 = null);

    Task<ImageEntity?> GetWithoutContent(ImageEntityId imageId);
    
    Task<List<ImageEntity>> GetManyWithoutContent(int skip, int take);

    Task<ImageEntity?> Get(ImageEntityId imageId);
    
    Task<string> GetPartialImageData(ImageEntityId imageId, int skip, int take);
    
    Task<ImageEntity?> Delete(ImageEntityId imageId);
}