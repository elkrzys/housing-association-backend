using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public record Issue : Event
    {
        [ForeignKey("Local")] public int? SourceLocalId { get; set; }
        [ForeignKey("Building")] public int SourceBuildingId { get; set; }
        [ForeignKey("PreviousIssue")] public int? PreviousIssueId { get; set; }
        public DateTimeOffset? Resolved { get; set; }
        public DateTimeOffset? Cancelled { get; set; }
        public Local Local { get; set; }
        public Building Building { get; set; }
        public Issue PreviousIssue { get; set; }
    }
}