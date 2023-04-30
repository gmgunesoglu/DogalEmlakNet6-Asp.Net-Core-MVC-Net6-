using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DogalEmlak.Web.Entities
{
    public class Role
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [StringLength(20)]
        public string Authority { get; set; }

        public Role(Guid userId, string authority)
        {
            UserId = userId;
            Authority = authority;
        }
    }
}
