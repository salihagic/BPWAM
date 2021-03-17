namespace BPWA.Common.Configuration
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public bool AutoMigrate { get; set; }
        public bool RecreateDatabase { get; set; }
        public bool Seed { get; set; }
    }
}
