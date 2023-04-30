using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Models
{
	public class AddPropertyModel : PropertyModel
	{
		public List<IFormFile> Files { get; set; } = new List<IFormFile>();
	}

}
