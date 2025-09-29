using System.Xml.Linq;
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

        public async Task<List<OrderModel>> getOrderAsync(int id)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<OrderModel>();
            string sql = @$"
                    SELECT o.id as IdOrder,
                  o.customer_id as IdCustomer,
                  c.name as Customer_name,
                  c.phone as Phone,
                  c.address as Address,
                  o.service_id as ServiceId,
                  t.id as Technical_id,
                  o.status as status,
                  oe.id as equipmentId,
                  oe.quantity as quantity,
                  e.unit,
                  e.price,
                  e.name as equipment_name,
                  o.schedule_time as ScheduleTime,
                  o.created_at,
                  o.updated_at
                  FROM orders o 
                  LEFT JOIN users c ON o.customer_id = c.id
                  LEFT JOIN users t ON o.technician_id = t.id
                  LEFT JOIN services s ON o.service_id = s.id
                  LEFT JOIN orderequipments oe ON o.id = oe.order_id
                  LEFT JOIN equipments e ON oe.equipment_id = e.id
                  WHERE o.id = {id}
            ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new OrderModel
                {
                    IdOrder = reader.GetInt16("IdOrder"),
                    IdCustomer = reader.GetInt16("IdCustomer"),
                    CustomerName = reader.GetString("Customer_name"),
                    Phone = reader.GetString("Phone"),
                    Address = reader.GetString("Address"),
                    ServiceId = reader.GetInt16("ServiceId"),
                    TechnicianId = reader.GetInt16("Technical_id"),
                    Status = reader.GetString("status"),
                    AppointmentTime = reader.GetDateTime("ScheduleTime"),
                    CreatedDate = reader.GetDateTime("created_at"),
                    UpdatedDate = reader.GetDateTime("updated_at"),
                    Items = new List<Equipments>
                        {
                            new Equipments { 
                                IdProduct = reader.GetInt16("equipmentId"), 
                                Name = reader.GetString("equipment_name"), 
                                Quantity = reader.GetInt16("quantity"),
                                Unit = reader.GetString("unit"),
                                Price=reader.GetInt32("price") },
                        }
                });
            }
            Console.Write(sql);
            return list;

        }

    }
}
