using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PracticalNineteen.Domain.Entities
{
    public class UserIdentity : IdentityUser
    {

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

    }
}
