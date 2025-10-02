using System.Threading.Tasks;

namespace HomeTechBlazor.Managers
{
    public class ChatbotManager
    {
        // Đây là chatbot demo, bạn có thể thay bằng API thật
        public async Task<string> AskAsync(string question)
        {
            await Task.Delay(300); // giả lập gọi API

            if (string.IsNullOrWhiteSpace(question))
                return "Xin chào! Bạn muốn hỏi gì?";

            if (question.Contains("dịch vụ"))
                return "TECHFIX cung cấp dịch vụ sửa chữa điện, nước, điện lạnh và CNTT.";

            if (question.Contains("giá"))
                return "Giá dịch vụ phụ thuộc vào yêu cầu cụ thể. Bạn có thể để lại thông tin để được báo giá chi tiết.";

            return $"Tôi chưa hiểu rõ câu hỏi: \"{question}\". Bạn có thể hỏi lại chi tiết hơn?";
        }
    }
}
