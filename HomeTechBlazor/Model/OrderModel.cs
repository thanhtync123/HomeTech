using HomeTechBlazor.Model;
using static HomeTechBlazor.Components.Pages.Orders.Order_detail;
namespace HomeTechBlazor.Model
{
    public class OrderModel
    {
        public int IdOrder { get; set; } = 0;

        public int IdCustomer { get; set; } = 0;
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";

        public string Address { get; set; } = "";
        public int ServiceId { get; set; } = 0;
        public int TechnicianId { get; set; } = 0;
        public string Status { get; set; } = "";
        public DateTime AppointmentTime { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public List<Equipments>? Items { get; set; } = new();
    }
}
