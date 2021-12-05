using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public record Issue : Event
    {
        [ForeignKey(nameof(Local))] public int? SourceLocalId { get; set; }
        [ForeignKey(nameof(Building))] public int SourceBuildingId { get; set; }
        public bool IsResolvedOrCancelled { get; set; }

        public Local Local { get; set; }
        public Building Building { get; set; }
    }
}