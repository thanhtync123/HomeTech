using HomeTechBlazor.Model;
using MySqlConnector;
namespace HomeTechBlazor.Service
{
    public class OrderDetailService:BaseService
    {
        public OrderDetailService(IConfiguration config) : base(config) { }
        public async Task<List<Equipments>> getEquipmentAsync(string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Equipments>();
            string sql = @$"
                Select id, name, unit, price, quantity, description 
                from equipments
                where name LIKE '%{keyword}%' or id LIKE '%{keyword}%'
                order by name
            ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Equipments
                {
                    IdProduct = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Unit = reader.GetString("unit"),
                    Quantity = reader.GetInt32("quantity"),
                    Price = reader.GetInt32("price")
                });
            }
            Console.Write(sql);
            return list;


        }
    }
}
