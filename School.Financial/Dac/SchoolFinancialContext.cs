using MySql.Data.MySqlClient;

namespace School.Financial.Dac
{
    public class SchoolFinancialContext
    {
        public string connectionString { get; set; }

        public SchoolFinancialContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
