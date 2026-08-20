using MySqlConnector;

namespace StudentRecordApp.Data
{
    public class Database
    {
        private readonly string connectionString =
            "Server=localhost;Database=StudentRecordDB;User ID=root;Password=f24bsse3814;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
