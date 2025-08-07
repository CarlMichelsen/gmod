namespace Presentation.Dto.Nav;

public record CreateNavNodeDto(
    List<Guid> LinkedNodes,
    string Map,
    string Tag,
    int X,
    int Y,
    int Z);