namespace DataAccess.Settings
{
    public class DbSettings
    {
        public string DbHost { get; set; }
        public string DbPort { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }
        public string PostgresUser { get; set; }
        public string PostgresPass { get; set; }
        public string ConnectionString => $"User ID={DbUser};Password={DbPass};Host={DbHost};Port={DbPort};Database={DbName};";
        public string MigrationConnectionString => $"User ID={PostgresUser};Password={PostgresPass};Host={DbHost};Port={DbPort};Database={DbName};";
    }
}
