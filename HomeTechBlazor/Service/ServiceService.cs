using MySqlConnector;
using HomeTechBlazor.Model;
namespace HomeTechBlazor.Service
{
    public class ServiceService:BaseService
    {
    

        public ServiceService(IConfiguration config) : base(config) { }
        public async Task<List<Services>> GetAllAsync(int offset,int pagesize,string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Services>();

            string sql = @$"
            SELECT *
            FROM services
            Where Name LIKE '%{keyword}%'
            ORDER by id DESC
            LIMIT {offset},{pagesize}
        ";
            var cmd = new MySqlCommand(sql, conn);
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Services
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                    Price = reader.GetInt32("price"),
                    Unit = reader.GetString("unit")

                });
            }
            int i = 0;
            Console.WriteLine("SQL Debug: " + sql + i++);
            return list;
        }
        public async Task<int> CountAsync(string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @$"
                SELECT COUNT(*) 
                FROM services
                Where Name LIKE '%{keyword}%'
            ";
            var cmd = new MySqlCommand(sql, conn);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task CreateSync(Services s)
        {

            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
            INSERT INTO services (name, description, price, unit) 
            VALUES ('{s.Name}', '{s.Description}', {s.Price}, '{s.Unit}');
            ";
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task DeleteSync(int id)
        {

            await using var conn = await GetOpenConnectionAsync();
            string sql = $@"
            DELETE FROM Services where id = {id};
            ";
            Console.WriteLine("xoa"+sql);
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
            
        }
        public async Task UpdateSync(Services s)
        {
            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
                UPDATE Services 
                SET name = '{s.Name}' ,
                description = '{s.Description}',
                price = {s.Price},
                unit = '{s.Unit}'
                WHERE id = {s.Id}
            ";
            Console.WriteLine("sua" + sql);
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }


    }
}
