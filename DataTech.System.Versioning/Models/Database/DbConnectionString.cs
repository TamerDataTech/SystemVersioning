namespace DatabaseUtils.Model.Database
{
    public class DbConnectionString
    {
        public DbConnectionString()
        {
            ConnectionTimeout = 5;
        }


        public DbConnectionString(DbConnectionString connectionString)
        {
            this.DataSource = connectionString.DataSource; 
            this.UserId = connectionString.UserId;
            this.Password = connectionString.Password;
            this.WindowsAuthenticaton = connectionString.WindowsAuthenticaton;
            ConnectionTimeout = 5;
        }



        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int ConnectionTimeout { get; set; } 
        public bool WindowsAuthenticaton { get; set; }


        public string ConnectionString { get; set; }

        public string GetConnectionString()
        {
            return WindowsAuthenticaton ? string.Format(
                "data source={0};initial catalog={1};integrated security=True;MultipleActiveResultSets=True;Connection Timeout={2}",
                DataSource, InitialCatalog, ConnectionTimeout) : string.Format(
                "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Connection Timeout={4}",
                DataSource, InitialCatalog, UserId, Password, ConnectionTimeout);
        }

        public string GetConnectionStringNoCatalog()
        {
            return WindowsAuthenticaton ? string.Format(
                 "data source={0};integrated security=True;MultipleActiveResultSets=True;Connection Timeout={1}",
                 DataSource, ConnectionTimeout) : string.Format(
                 "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Connection Timeout={3}",
                 DataSource, UserId, Password, ConnectionTimeout);
        }

        

        public bool IsValid()
        {
            return   (!string.IsNullOrEmpty(DataSource) && !string.IsNullOrEmpty(InitialCatalog) &&
                   !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Password));
        }

        public bool IsValidNoCatalog()
        {
            return   (!string.IsNullOrEmpty(DataSource) &&
                   !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Password));
        }


    }
}
