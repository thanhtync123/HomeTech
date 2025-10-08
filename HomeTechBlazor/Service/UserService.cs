using HomeTechBlazor.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeTechBlazor.Service
{
    public class UserService : BaseService
    {
        public UserService(IConfiguration config) : base(config) { }

        // --- Lấy danh sách người dùng (ĐÃ BẢO MẬT) ---
        public async Task<List<Users>> GetAllAsync(int offset, int pagesize, string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            var list = new List<Users>();

            string sql = @"
                SELECT id, name, phone, password, address, role, created_at, updated_at 
                FROM users
                WHERE id LIKE @keyword OR phone LIKE @keyword OR role LIKE @keyword
                ORDER BY id DESC
                LIMIT @offset, @pagesize";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@pagesize", pagesize);

            await using var reader = await cmd.ExecuteReaderAsync();
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
            return list;
        }

        // --- Đếm số lượng người dùng (ĐÃ BẢO MẬT) ---
        public async Task<int> CountAsync(string keyword)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @"
                SELECT COUNT(*)
                FROM users
                WHERE id LIKE @keyword OR phone LIKE @keyword OR role LIKE @keyword";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // --- Tạo người dùng mới (ĐÃ BẢO MẬT) ---
        public async Task CreateAsync(Users u)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @"
                INSERT INTO users (name, phone, password, address, role) 
                VALUES (@Name, @Phone, @Password, @Address, @Role)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password); // LƯU Ý: Mật khẩu nên được mã hóa (hashed)
            cmd.Parameters.AddWithValue("@Address", u.Address);
            cmd.Parameters.AddWithValue("@Role", u.Role);

            await cmd.ExecuteNonQueryAsync();
        }

        // --- Đăng ký người dùng (ĐÃ BẢO MẬT) ---
        public async Task RegisterAsync(Users u)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @"
                INSERT INTO users (name, phone, password, address, role) 
                VALUES (@Name, @Phone, @Password, @Address, 'customer')"; // Mặc định role là 'customer'

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Address", u.Address);

            await cmd.ExecuteNonQueryAsync();
        }

        // --- Cập nhật thông tin người dùng (ĐÃ BẢO MẬT) ---
        public async Task UpdateAsync(Users u)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = @"
                UPDATE users
                SET 
                    name = @Name,
                    phone = @Phone,
                    password = @Password,
                    address = @Address,
                    role = @Role
                WHERE id = @Id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Address", u.Address);
            cmd.Parameters.AddWithValue("@Role", u.Role);
            cmd.Parameters.AddWithValue("@Id", u.Id);

            await cmd.ExecuteNonQueryAsync();
        }

        // --- Xóa người dùng (ĐÃ BẢO MẬT) ---
        public async Task DeleteAsync(int id)
        {
            await using var conn = await GetOpenConnectionAsync();
            string sql = "DELETE FROM users WHERE id = @Id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        // === PHƯƠNG THỨC MỚI ĐỂ CẬP NHẬT KHUÔN MẶT (ĐÃ BẢO MẬT) ===
        public async Task<bool> UpdateFaceDescriptorAsync(int userId, string faceDescriptorJson)
        {
            try
            {
                await using var conn = await GetOpenConnectionAsync();

                string sql = "UPDATE users SET FaceDescriptor = @FaceDescriptor WHERE Id = @UserId";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FaceDescriptor", faceDescriptorJson);
                cmd.Parameters.AddWithValue("@UserId", userId);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật face descriptor: {ex.Message}");
                return false;
            }
        }
    }
}