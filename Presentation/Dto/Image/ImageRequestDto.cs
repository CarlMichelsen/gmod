namespace Presentation.Dto.Image;

public record ImageRequestDto(
    Uri Source,
    string ImageName,
    int SquareResolution,
    ulong CreatorSteamId64);