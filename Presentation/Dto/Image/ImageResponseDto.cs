namespace Presentation.Dto.Image;

public record ImageResponseDto(
    Guid ImageId,
    SizeDto Size,
    ulong? CreatorSteamId64,
    int ContentHashCode,
    Uri Source);