using DogalEmlak.Web.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DogalEmlak.Web.Models
{
	public class PropertyModel : PropertySummaryModel
	{

		[Required(ErrorMessage = "Adres bilgisi girilmedi!")]
		[MaxLength(200, ErrorMessage = "Adres en fazla 200 karakter olmalıdır!")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Brüt hacmi girilmedi!")]
		[MaxLength(5, ErrorMessage = "Brüt hacmi en fazla 5 karakter olmalıdır!")]
		public string SizeOfGross { get; set; }

		public DateTime DateOfAdded { get; set; }
	}
}

//direk olarak bir view de kullanılmayacak ama ara sınıf olarak gerekli
