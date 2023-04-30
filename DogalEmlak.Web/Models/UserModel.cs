using DogalEmlak.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Models
{
    public class UserModel : CreateUserModel
    {
        public Guid Id { get; set; }

        public bool Locked { get; set; } = false;

        public DateTime DateOfAdded { get; set; } = DateTime.Now;

        public List<Role> Roles { get; set; }

		public UserModel()
		{
		}

		public UserModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Password = user.Password;
            Locked = user.Locked;
            DateOfAdded = user.DateOfAdded;
            Roles = user.Roles;
        }
    }
}
