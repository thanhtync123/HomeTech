using System.Data;
using System.Xml.Linq;
//using HomeTechBlazor.Components.Pages.Orders;
using HomeTechBlazor.Model;
using MySqlConnector;

namespace HomeTechBlazor.Service
{
    public class OrderDetailService : BaseService
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
          e.id as EquipmentId, 
          t.id as Technical_id,
          o.status as status,
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
            OrderModel order = null;

            while (await reader.ReadAsync())
            {
                if (order == null)
                {
                    order = new OrderModel
                    {
                        IdOrder = reader.SafeGetInt("IdOrder"),
                        CustomerName = reader.SafeGetString("Customer_name"),
                        IdCustomer = reader.SafeGetInt("IdCustomer"),
                        Phone = reader.SafeGetString("Phone"),
                        Address = reader.SafeGetString("Address"),
                        ServiceId = reader.SafeGetInt16("ServiceId"),
                        TechnicianId = reader.SafeGetInt16("Technical_id"),
                        Status = reader.SafeGetString("status"),
                        AppointmentTime = reader.SafeGetDateTime("ScheduleTime"),
                        CreatedDate = reader.SafeGetDateTime("created_at"),
                        UpdatedDate = reader.SafeGetDateTime("updated_at"),
                        Items = new List<Equipments>()
                    };
                    list.Add(order);
                }
                if (!reader.IsDBNull(reader.GetOrdinal("equipmentId")))
                {
                    order.Items.Add(new Equipments
                    {
                        IdProduct = reader.GetInt16("EquipmentId"),
                        Name = reader.GetString("equipment_name"),
                        Quantity = reader.GetInt16("quantity"),
                        Unit = reader.GetString("unit"),
                        Price = reader.GetInt32("price")
                    });
                }
            }

            Console.Write(sql);
            return list;
        }
        public async Task<Dictionary<int, string>> getTechnicalSelect()
        {
            await using var conn = await GetOpenConnectionAsync();
            var dict = new Dictionary<int, string>();
            string sql = @"
                SELECT id, name 
                FROM users 
                WHERE role = 'technical'
                 ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int userId = Convert.ToInt32(reader["id"]);
                string fullName = reader["name"].ToString();
                dict[userId] = fullName;
            }

            Console.WriteLine(sql);
            return dict;
        }
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

            Console.WriteLine(sql);
            return dict;
        }
        public async Task SaveOrderExamSync(OrderModel om)
        {
            await using var conn = await GetOpenConnectionAsync();

            string sql = $@"
             update orders
                set 
                    service_id = {om.ServiceId}   , 
                    technician_id = {om.TechnicianId}, 
                    schedule_time = '{om.AppointmentTime:yyyy-MM-dd HH:mm:ss}',
                    status = '{om.Status}', 
                    total_price = {om.totalPrice} 
                where id = {om.IdOrder}
                        ";
            var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();


            sql = $"DELETE FROM orderequipments WHERE order_id = {om.IdOrder}";
            Console.WriteLine("del sql" + sql);
            cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
            foreach (var item in om.Items)
            {
                sql = $@"
                     INSERT INTO orderequipments(order_id, equipment_id, quantity) VALUE ({om.IdOrder}, {item.IdProduct},{item.Quantity})
                    ";
                Console.WriteLine("insert sql"+sql );
                cmd = new MySqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }    

        }

        public async Task<List<Users>> getCustomerAsync()
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Users>();
            //id, name, phone, password, address, role, created_at, updated_at
            string sql = @$"
                select * from users where role = 'customer'
            ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Users
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Phone = reader.GetString("phone"),
                    Address = reader.GetString("address")
                });
            }
            Console.Write(sql);
            return list;


        }
        public async Task<int> getMaxOrderId()
        {
            await using var conn = await GetOpenConnectionAsync();
            int id=0;
            string sql = @$"
                select id from orders order by id desc limit 1
            ";
            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                id = reader.GetInt16(id);
            }
            return id;
        }
        public async Task CreateOrderExamSync(OrderModel om)
        {
            await using var conn = await GetOpenConnectionAsync();
            // id, customer_id, service_id, technician_id, schedule_time, status, total_price, created_at, updated_at
            string sql = $@"
    insert into orders (customer_id, service_id, technician_id, schedule_time, status, total_price)
    values ({om.IdCustomer},{om.ServiceId},{om.TechnicianId},'{om.AppointmentTime?.ToString("yyyy-MM-dd HH:mm:ss")}','{om.Status}',{om.totalPrice})
                ";
            var cmd = new MySqlCommand(sql, conn);
            Console.WriteLine("insert sql orders" + sql);
            await cmd.ExecuteNonQueryAsync();

            foreach (var item in om.Items)
            {
                sql = $@"
                     INSERT INTO orderequipments(order_id, equipment_id, quantity) VALUE ({om.IdOrder}, {item.IdProduct},{item.Quantity})
                    ";


                Console.WriteLine("insert sql orderequipments" + sql);
                cmd = new MySqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }


    }

}

