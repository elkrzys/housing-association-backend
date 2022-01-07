namespace HousingAssociation.Settings
{
    public class DbSettings
    {
        public string DbHost { get; set; }
        public string DbPort { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }
        public string ConnectionString => $"User ID={DbUser};Password={DbPass};Host={DbHost};Port={DbPort};Database={DbName};";
    }
}
