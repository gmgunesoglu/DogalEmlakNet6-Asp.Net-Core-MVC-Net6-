using DogalEmlak.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Entities
{
    public class Property
    {
        [Key]
        public Guid Id { get; set; }

		[StringLength(30)]
		public string Header { get; set; }

		public int Price { get; set; }

		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(20)]
		public string TypeOfProperty { get; set; }

		[StringLength(20)]
		public string TypeOfUsage { get; set; }

		[StringLength(5)]
		public string Rooms { get; set; }

		[StringLength(5)]
		public string SizeOfNet { get; set; }

		[StringLength(5)]
		public string SizeOfGross { get; set; }

		public DateTime DateOfAdded { get; set; }

		public DateTime DateOfRenewal { get; set; }

		public List<PropertyImage> PropertyImages { get; set; }

		public Property()
		{
		}

		public Property(PropertyModel model)
		{
			Header = model.Header;
			Price = model.Price;
			Address = model.Address;
			TypeOfProperty = model.TypeOfProperty;
			TypeOfUsage = model.TypeOfUsage;
			Rooms = model.Rooms;
			SizeOfNet = model.SizeOfNet;
			SizeOfGross = model.SizeOfGross;
			DateOfAdded = model.DateOfAdded;
			DateOfRenewal = model.DateOfRenewal;
		}


		public PropertyModel ToModel()
		{
			PropertyModel model = new PropertyModel();
			model.Id = Id;
			model.Header = Header;
			model.Price = Price;
			model.Address = Address;
			model.TypeOfProperty = TypeOfProperty;
			model.TypeOfUsage = TypeOfUsage;
			model.Rooms = Rooms;
			model.SizeOfNet = SizeOfNet;
			model.SizeOfGross = SizeOfGross;
			model.DateOfAdded = DateOfAdded;
			model.DateOfRenewal = DateOfRenewal;
			return model;
		}

		public PropertySummaryModel ToSummarty()
		{
			PropertySummaryModel model = new PropertySummaryModel();
			model.Id = Id;
			model.Header = Header;
			model.Price = Price;
			model.TypeOfProperty = TypeOfProperty;
			model.TypeOfUsage = TypeOfUsage;
			model.Rooms = Rooms;
			model.SizeOfNet = SizeOfNet;
			model.DateOfRenewal = DateOfRenewal;
			return model;
		}

	}
}
