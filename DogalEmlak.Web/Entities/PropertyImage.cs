using DogalEmlak.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DogalEmlak.Web.Entities
{
	public class PropertyImage
	{
		[ForeignKey("User")]
		public Guid PropertyId { get; set; }

		[StringLength(200)]//bu 20 iken hata aldım çok uğraştım:)
		public string FileName { get; set; }

		public byte[] ImageData { get; set; }

		public PropertyImage()
		{
		}

		public PropertyImage(Guid propertyId, byte[] ımageData, string fileName)
		{
			PropertyId = propertyId;
			ImageData = ımageData;
			FileName = fileName;
		}
	}
}
