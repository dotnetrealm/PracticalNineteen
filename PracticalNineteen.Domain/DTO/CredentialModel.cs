using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PracticalNineteen.Domain.DTO
{
    public class CredentialModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "email must be less than or equals to 256 characters.")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Please enter valid email address!")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [StringLength(128, ErrorMessage = "Password must be less than or equals to 128 characters.")]
        public string Password { get; set; } = null!;

        [DisplayName("Remeber Me")]
        public bool RememberMe { get; set; }
    }
}
