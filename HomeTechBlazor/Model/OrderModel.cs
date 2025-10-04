using System.ComponentModel.DataAnnotations;
using HomeTechBlazor.Model;
//using static HomeTechBlazor.Components.Pages.Order_detail;
namespace HomeTechBlazor.Model
{
    public class OrderModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn khách hàng")]
        public int IdOrder { get; set; } = 0;

        [Required(ErrorMessage = "IdCustomer là bắt buộc.")]
        public int IdCustomer { get; set; } = 0;

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được quá 100 ký tự.")]
        public string CustomerName { get; set; } = "";

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự.")]
        public string Address { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn dịch vụ.")]
        public int ServiceId { get; set; } = 0;

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn kỹ thuật viên.")]
        public int TechnicianId { get; set; } = 0;

        [Required(ErrorMessage = "Status là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Status không được quá 50 ký tự.")]
        public string Status { get; set; } = "";

        [Range(0, int.MaxValue, ErrorMessage = "TotalPrice phải >= 0.")]
        public int? totalPrice { get; set; } = 0;

        public DateTime? AppointmentTime { get; set; } = DateTime.Now;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public List<Equipments>? Items { get; set; } = new();
    }
}
