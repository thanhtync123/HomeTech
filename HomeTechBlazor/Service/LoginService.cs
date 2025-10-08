using HomeTechBlazor.Model;
using MySqlConnector;
using System.Text.Json;

namespace HomeTechBlazor.Service
{
    public class LoginService : BaseService
    {
        public LoginService(IConfiguration config) : base(config) { }

        // --- PHƯƠNG THỨC ĐĂNG NHẬP BẰNG MẬT KHẨU (ĐÃ TỐI ƯU BẢO MẬT) ---
        public async Task<string?> LoginAsync(string username, string password)
        {
            await using var conn = await GetOpenConnectionAsync();

            // SỬ DỤNG TRUY VẤN THAM SỐ ĐỂ BẢO MẬT HƠN
            string sql = @"
                SELECT role, name, phone, address, password, id 
                FROM users 
                WHERE phone = @phone AND password = @password
                LIMIT 1";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@phone", username);
            cmd.Parameters.AddWithValue("@password", password); // Lưu ý: Mật khẩu nên được mã hóa (hashed)

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var userInfo = new
                {
                    id = reader.GetInt16("id").ToString(),
                    role = reader.GetString("role"),
                    name = reader.GetString("name"),
                    phone = reader.GetString("phone"),
                    address = reader.GetString("address"),
                    dbPassword = reader.GetString("password")
                };
                return JsonSerializer.Serialize(userInfo);
            }
            else
            {
                return null;
            }
        }

        // --- PHƯƠNG THỨC XÁC THỰC KHUÔN MẶT ---
        public async Task<string?> VerifyFaceLoginAsync(float[] loginDescriptor)
        {
            await using var conn = await GetOpenConnectionAsync();

            string sql = "SELECT id, role, name, phone, address, password, FaceDescriptor FROM users WHERE FaceDescriptor IS NOT NULL AND FaceDescriptor != ''";

            var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var faceDescriptorString = reader.GetString("FaceDescriptor");
                var storedDescriptor = JsonSerializer.Deserialize<float[]>(faceDescriptorString);

                if (storedDescriptor != null)
                {
                    var distance = CalculateEuclideanDistance(loginDescriptor, storedDescriptor);

                    if (distance < 0.4) // Ngưỡng nhận diện
                    {
                        var userInfo = new
                        {
                            id = reader.GetInt16("id").ToString(),
                            role = reader.GetString("role"),
                            name = reader.GetString("name"),
                            phone = reader.GetString("phone"),
                            address = reader.GetString("address"),
                            dbPassword = reader.GetString("password")
                        };

                        await reader.CloseAsync();
                        return JsonSerializer.Serialize(userInfo);
                    }
                }
            }
            return null; // Không tìm thấy user nào khớp
        }

        // --- PHƯƠNG THỨC HỖ TRỢ TÍNH TOÁN ---
        private double CalculateEuclideanDistance(float[] desc1, float[] desc2)
        {
            if (desc1 == null || desc2 == null || desc1.Length != desc2.Length)
            {
                return double.MaxValue; // Trả về giá trị lớn để không bao giờ khớp
            }

            double sum = 0;
            for (int i = 0; i < desc1.Length; i++)
            {
                sum += Math.Pow(desc1[i] - desc2[i], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}