using Microsoft.Data.SqlClient;
using System.Data;

namespace Api_DataPeople.Data
{
    public class SqlConnectionFactory:ISqlConnectionFactory
    {
        private readonly String _connectionString;

        public SqlConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("falta de cadena de conexion");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
