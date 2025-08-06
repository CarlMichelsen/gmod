using Database.Util;

namespace Database.Entity.Id;

public class ImageContentEntityId(Guid value, bool allowWrongVersion = false)
    : TypedGuid<ImageContentEntity>(value, allowWrongVersion);