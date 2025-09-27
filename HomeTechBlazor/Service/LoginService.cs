using HomeTechBlazor.Model;
using MySqlConnector;

namespace HomeTechBlazor.Service
{
    public class LoginService:BaseService
    {
        public LoginService(IConfiguration config) : base(config) { }
        public async Task<string?> LoginAsync(string username, string password)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @$"
                 select role,name,phone,address 
                from users 
                where phone = '{username}' and password = '{password}'
                limit 1
            ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var role = reader.GetString("role");
                var name = reader.GetString("name");
                var phone = reader.GetString("phone");
                var address = reader.GetString("address");
                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    role,
                    name,
                    phone,
                    address
                });
            }
            else
                return null; 
            
        }
    }
}
