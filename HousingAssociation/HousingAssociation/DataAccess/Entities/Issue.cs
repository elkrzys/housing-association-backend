using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public record Issue : Event
    {
        [ForeignKey("Local")] public int? SourceLocalId { get; set; }
        [ForeignKey("Building")] public int SourceBuildingId { get; set; }
        public bool IsResolved { get; set; }
        public bool IsCancelled { get; set; }

        public Local Local { get; set; }
        public Building Building { get; set; }
    }
}