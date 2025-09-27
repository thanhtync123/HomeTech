using System.ComponentModel.DataAnnotations;

namespace HomeTechBlazor.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = string.Empty;
        public string? Name { get; set; }

        public string? Address { get; set; }


    }
}