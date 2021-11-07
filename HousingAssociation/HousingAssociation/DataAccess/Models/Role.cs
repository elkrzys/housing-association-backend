using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HousingAssociation.DataAccess.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        Admin,
        Worker,
        Resident
    }
}