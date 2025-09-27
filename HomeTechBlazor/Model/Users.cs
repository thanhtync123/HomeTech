using System.ComponentModel.DataAnnotations;

namespace HomeTechBlazor.Model
{
    public class Users
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [RegularExpression("^(admin|customer|technical)$", ErrorMessage = "Vai trò phải là admin, customer hoặc technical")]
        public string Role { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
