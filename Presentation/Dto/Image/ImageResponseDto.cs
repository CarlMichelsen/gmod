namespace Presentation.Dto.Image;

public record ImageResponseDto(
    Guid ImageId,
    SizeDto Size,
    string Name,
    string? CreatorSteamId64,
    string? ContentHashCode,
    Uri Source);