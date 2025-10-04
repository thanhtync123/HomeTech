using HomeTechBlazor.Model;
using MySqlConnector;

namespace HomeTechBlazor.Service
{
    public class BookingService:BaseService
    {
        public BookingService(IConfiguration config) : base(config) { }
        public async Task<Dictionary<int, (string name, int price)>> getServiceSelect()
        {
            await using var conn = await GetOpenConnectionAsync();
            var dict = new Dictionary<int, (string name, int price)>();
            string sql = @"
                select id,name,price from services order by name
                 ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int id = reader.SafeGetInt("id");
                string name = reader.SafeGetString("name");
                int price = reader.SafeGetInt("price");
                dict[id] = (name, price);
            }
            return dict;
        }
        public async Task CreateBooking(OrderModel o)
        {
            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
    INSERT INTO orders (customer_id, service_id, schedule_time) 
    VALUES ({o.IdCustomer}, {o.ServiceId}, '{o.AppointmentTime:yyyy-MM-dd HH:mm:ss}')
";

            
            Console.Write("sql:"+sql);
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

        }
       
    }
}
