using Database.Entity;
using Presentation.Dto.Nav;

namespace Application.Mapper;

public static class NavNodeMapper
{
    public static NavNodeDto ToDto(this NavNodeEntity imageEntity)
        => new NavNodeDto(
            Id: imageEntity.Id.Value,
            LinkedTo: imageEntity.LinkedTo.Select(x => x.Id.Value).ToList(),
            LinkedFrom: imageEntity.LinkedFrom.Select(x => x.Id.Value).ToList(),
            X: imageEntity.X,
            Y: imageEntity.Y,
            Z: imageEntity.Z,
            Tag: imageEntity.Tag,
            Map: imageEntity.Map);
}