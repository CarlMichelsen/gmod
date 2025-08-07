namespace Presentation.Dto.Nav;

public record NavNodeLinkDto(
    Guid NodeAId,
    Guid NodeBId,
    string Map,
    string Tag);