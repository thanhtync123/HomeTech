using HomeTechBlazor.Model;
using MySqlConnector;

namespace HomeTechBlazor.Service
{
    public class UserService:BaseService
    {
        public UserService(IConfiguration config) : base(config) { }
        public async Task<List<Users>> GetAllAsync(int offset, int pagesize, string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Users>();

            string sql = @$"
               select id, name, phone, password, address, role, created_at, updated_at 
                from users
                where id LIKE '%{keyword}%'
                or phone LIKE '%{keyword}%'
                or role LIKE '%{keyword}%'
                ORDER BY id DESC
                LIMIT {offset},{pagesize}
                ";
            var cmd = new MySqlCommand(sql, conn);
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Users
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Phone = reader.GetString("phone"),
                    Password = reader.GetString("password"),
                    Address = reader.GetString("address"),
                    Role = reader.GetString("role"),
                });
            }
            Console.WriteLine("co cai quan"+sql);
            return list;
        }
        public async Task<int> CountAsync(string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @$"
                select count(*)
                from users
                where id LIKE '%{keyword}%'
                or phone LIKE '%{keyword}%'
                or role LIKE '%{keyword}%'
                ORDER BY id DESC
            ";
            var cmd = new MySqlCommand(sql, conn);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task CreateSync(Users u)
        {

            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
            INSERT INTO users (name, phone, password, address, role) VALUES
            ('{u.Name}', '{u.Phone}', '{u.Password}', '{u.Address}', '{u.Role}')
            ";
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task RegisterSync(Users u)
        {

            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
            INSERT INTO users (name, phone, password, address) VALUES
            ('{u.Name}', '{u.Phone}', '{u.Password}', '{u.Address}')
            ";
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task UpdateSync(Users u)
        {

            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
            UPDATE users
            SET 
                name = '{u.Name}',
                phone = '{u.Phone}',
                password = '{u.Password}',
                address = '{u.Address}',
                role = '{u.Role}'
            WHERE id = {u.Id};

            ";
            Console.WriteLine("Tsadfjk" + sql);
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task DeleteSync(int id)
        {

            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
                DELETE FROM users where id = {id}
            ";
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }

    }
}
