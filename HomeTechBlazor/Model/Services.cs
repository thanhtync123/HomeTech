using System.ComponentModel.DataAnnotations;

namespace HomeTechBlazor.Model
{
    public class Services
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên dịch vụ không được quá 100 ký tự")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1000, 10000000, ErrorMessage = "Giá phải nằm trong khoảng 1.000 - 10.000.000")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        [StringLength(50, ErrorMessage = "Đơn vị không được quá 50 ký tự")]
        public string Unit { get; set; }
    }
}
