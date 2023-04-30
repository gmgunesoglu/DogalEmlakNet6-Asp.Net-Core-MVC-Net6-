using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Kullanıcı adı girlmedi!")]
        [MinLength(8, ErrorMessage = "Kullanıcı adı en az 8 karakter olmalıdır!")]
        [MaxLength(20, ErrorMessage = "Kullanıcı adı en fazla 20 karakter olmalıdır!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre adı girlmedi!")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır!")]
        [MaxLength(20, ErrorMessage = "Şifre en fazla 20 karakter olmalıdır!")]
        public string Password { get; set; }
    }
}
