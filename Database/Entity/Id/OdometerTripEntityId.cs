using Database.Util;

namespace Database.Entity.Id;

public class OdometerTripEntityId(Guid value, bool allowWrongVersion = false)
    : TypedGuid<OdometerTripEntity>(value, allowWrongVersion);