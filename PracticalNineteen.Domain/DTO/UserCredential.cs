using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PracticalNineteen.Domain.DTO
{
    public class UserCredential
    {
        [Required]
        [StringLength(256, ErrorMessage = "User Name must be less than or equals to 256 characters.")]
        [DisplayName("User Name")]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [StringLength(128, ErrorMessage = "Password must be less than or equals to 128 characters.")]
        public string Password { get; set; } = null!;

        [DisplayName("Remeber Me")]
        public bool RememberMe { get; set; }
    }
}
