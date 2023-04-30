using DogalEmlak.Web.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DogalEmlak.Web.Models
{
	public class PropertySummaryModel
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Başlık Girilmedi!")]
		[MaxLength(30, ErrorMessage = "Başlık en fazla 30 karakter olmalıdır!")]
		public string Header { get; set; }

		[Required(ErrorMessage = "Fiyat bilgisi girilmedi!")]
		public int Price { get; set; }

		[Required(ErrorMessage = "Mülk tipi girilmedi!")]
		[MaxLength(20, ErrorMessage = "Mülk tipi en fazla 20 karakter olmalıdır!")]
		public string TypeOfProperty { get; set; }

		[Required(ErrorMessage = "Kullanım bilgisi girilmedi!")]
		[MaxLength(20, ErrorMessage = "Kullanım bilgisi en fazla 20 karakter olmalıdır!")]
		public string TypeOfUsage { get; set; }

		[Required(ErrorMessage = "Oda bilgisi girilmedi!")]
		[MaxLength(5, ErrorMessage = "Oda bilgisi en fazla 5 karakter olmalıdır!")]
		public string Rooms { get; set; }

		[Required(ErrorMessage = "Net hacmi girilmedi!")]
		[MaxLength(5, ErrorMessage = "Net hacmi en fazla 5 karakter olmalıdır!")]
		public string SizeOfNet { get; set; }

		public DateTime DateOfRenewal { get; set; }
	}
}

//ilanlar listelenirken gerekli bir model