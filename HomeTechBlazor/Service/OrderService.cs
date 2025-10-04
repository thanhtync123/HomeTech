using HomeTechBlazor.Model;
using MySqlConnector;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HomeTechBlazor.Service
{
    public class OrderService : BaseService
    {
        public OrderService(IConfiguration config) : base(config) { }

        public async Task<List<Orders>> GetAllAsync(int offset, int pagesize, string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Orders>();

            string sql = @$"
                SELECT 
                    o.id,
                    c.name AS customer_name,
                    t.name AS technician_name,
                    s.name AS service_name,
                    DATE_FORMAT(o.schedule_time, '%d/%m/%Y %H:%i') AS schedule_time,
                    o.status,
                    o.total_price,
                    DATE_FORMAT(o.created_at, '%d/%m/%Y %H:%i') AS created_at,
                    DATE_FORMAT(o.updated_at, '%d/%m/%Y %H:%i') AS updated_at
                FROM orders o
                JOIN services s ON o.service_id = s.id
                JOIN users c ON o.customer_id = c.id
                LEFT JOIN users t ON o.technician_id = t.id
                WHERE c.name LIKE '%{keyword}%'
                OR c.id LIKE '%{keyword}%'
                OR o.id LIKE '%{keyword}%'
                OR t.id LIKE '%{keyword}%'
                OR t.name LIKE '%{keyword}%'
                ORDER BY o.id DESC
                LIMIT {offset},{pagesize}
                ";
            var cmd = new MySqlCommand(sql, conn);
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Orders
                {
                    Id = reader.SafeGetInt("id"),
                    CustomerName = reader.SafeGetString("customer_name"),
                    TechnicianName = reader.SafeGetString("technician_name"),
                    ServiceName = reader.SafeGetString("service_name"), 
                    ScheduleTime = reader.SafeGetString("schedule_time"),
                    Status = reader.SafeGetString("status"),
                    TotalPrice = reader.SafeGetInt("total_price"),
                    CreatedAt = reader.SafeGetString("created_at"),
                    UpdatedAt = reader.SafeGetString("updated_at")

                });
            }
            return list;
        }
        public async Task<int> CountAsync(string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @$"
            SELECT COUNT(*) AS total_orders
            FROM orders o
            JOIN services s ON o.service_id = s.id
            JOIN users c ON o.customer_id = c.id
            LEFT JOIN users t ON o.technician_id = t.id
            WHERE c.name LIKE '%{keyword}%'
            OR c.id LIKE '%{keyword}%'
            OR o.id LIKE '%{keyword}%'
            OR t.id LIKE '%{keyword}%'
            OR t.name LIKE '%{keyword}%'
            ";
            var cmd = new MySqlCommand(sql, conn);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task UpdateStatusSync(Orders o)
        {
            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
                    UPDATE orders o
                    SET status = '{o.Status}'
                    WHERE o.id = {o.Id};
            ";
            Console.WriteLine("UpdeSTT"+sql);
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

    }

}
    

