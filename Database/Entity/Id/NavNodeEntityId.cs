using Database.Util;

namespace Database.Entity.Id;

public class NavNodeEntityId(Guid value, bool allowWrongVersion = false)
    : TypedGuid<NavNodeEntity>(value, allowWrongVersion);