namespace DogalEmlak.Web.Models
{
    public class UpdatePropertyModel : ShowPropertyModel
    {
		public UpdatePropertyModel()
		{
		}

		public UpdatePropertyModel(PropertyModel propertyModel, List<byte[]> ImageData) : base(propertyModel, ImageData)
        {
        }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

    }
}
