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
                 select role 
                from users 
                where phone = '{username}' and password = '{password}'
                limit 1
            ";
            var cmd = new MySqlCommand(sql, conn);
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString(); // null nếu không tìm thấy
        }
    }
}
