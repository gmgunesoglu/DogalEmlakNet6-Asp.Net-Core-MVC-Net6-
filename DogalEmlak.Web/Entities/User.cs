using System.ComponentModel.DataAnnotations;

namespace DogalEmlak.Web.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool Locked { get; set; } = false;

        public DateTime DateOfAdded { get; set; } = DateTime.Now;

        public List<Role> Roles { get; set; }

        public User(string firstName, string lastName, string userName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Password = password;
        }
    }
}
