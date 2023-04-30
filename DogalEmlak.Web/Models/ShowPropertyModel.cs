	namespace DogalEmlak.Web.Models
{
	public class ShowPropertyModel : PropertyModel
	{
		public List<byte[]> ImageData { get; set; }

		public ShowPropertyModel()
		{
		}

		public ShowPropertyModel(PropertyModel propertyModel, List<byte[]> ImageData) 
		{
			this.ImageData = ImageData;
			Id=propertyModel.Id;
			Price=propertyModel.Price;
			Header=propertyModel.Header;
			Address=propertyModel.Address;
			DateOfAdded=propertyModel.DateOfAdded;
			DateOfRenewal=propertyModel.DateOfRenewal;
			Rooms = propertyModel.Rooms;
			SizeOfGross=propertyModel.SizeOfGross;
			SizeOfNet=propertyModel.SizeOfNet;
			TypeOfProperty=propertyModel.TypeOfProperty;
			TypeOfUsage=propertyModel.TypeOfUsage;	
		}
	}
}
