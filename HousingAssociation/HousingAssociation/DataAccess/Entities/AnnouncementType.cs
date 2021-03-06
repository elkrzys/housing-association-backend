using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HousingAssociation.DataAccess.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnnouncementType
    {
        Issue,
        Announcement,
        Alert
    }
}