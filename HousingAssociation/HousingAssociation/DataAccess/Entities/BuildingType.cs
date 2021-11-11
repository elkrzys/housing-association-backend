using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HousingAssociation.DataAccess.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BuildingType
    {
        Block = 0,
        House = 1
    }
}