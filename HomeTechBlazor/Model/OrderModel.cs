using HomeTechBlazor.Model;
using static HomeTechBlazor.Components.Pages.Orders.Order_detail;
namespace HomeTechBlazor.Model
{
    public class OrderModel
    {
        public string IdOrder { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string ServiceId { get; set; } = "";
        public string TechnicianId { get; set; } = "";
        public string Status { get; set; } = "pending";
        public DateTime AppointmentTime { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public List<Equipments>? Items { get; set; } = new();
    }
}
