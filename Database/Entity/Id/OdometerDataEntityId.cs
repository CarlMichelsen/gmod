using Database.Util;

namespace Database.Entity.Id;

public class OdometerDataEntityId(Guid value, bool allowWrongVersion = false)
    : TypedGuid<OdometerDataEntity>(value, allowWrongVersion);