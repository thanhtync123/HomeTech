using MySqlConnector;
using Microsoft.Extensions.Configuration; 
namespace HomeTechBlazor.Service
{
    public abstract class BaseService
    {
        protected readonly string _connStr;

        protected BaseService(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }


        protected async Task<MySqlConnection> GetOpenConnectionAsync()
        {
            var conn = new MySqlConnection(_connStr);
            await conn.OpenAsync();
            return conn;
        }

        
    }
}
