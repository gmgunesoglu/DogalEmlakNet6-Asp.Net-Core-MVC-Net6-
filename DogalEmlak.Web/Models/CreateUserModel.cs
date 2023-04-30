using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Models
{
	public class CreateUserModel : LoginUserModel
	{
		[Required(ErrorMessage = "İsim girlmedi!")]
		[MaxLength(30, ErrorMessage = "İsim en fazla 30 karakter olmalıdır!")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Soy isim girlmedi!")]
		[MaxLength(20, ErrorMessage = "Soy isim en fazla 20 karakter olmalıdır!")]
		public string LastName { get; set; }
	}
}
