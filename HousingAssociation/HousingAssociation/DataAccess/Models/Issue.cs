namespace HousingAssociation.DataAccess.Models
{
    public class Issue : Event
    {
        public Local SourceLocal { get; set; }
        public Building SourceBuilding { get; set; }
        public bool IsResolvedOrCancelled { get; set; }
    }
}