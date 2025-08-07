namespace Presentation.Dto.Nav;

public record RemoveNavNodeDto(
    Guid Id,
    string Map,
    string Tag);