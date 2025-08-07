namespace Presentation.Dto.Nav;

public record NavNodeDto(
    Guid Id,
    List<Guid> LinkedTo,
    List<Guid> LinkedFrom,
    int X,
    int Y,
    int Z,
    string Tag,
    string Map);