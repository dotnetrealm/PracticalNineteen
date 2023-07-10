using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PracticalNineteen.Domain.DTO
{
    public class UserModel
    {
        //public string? Id { get; set; }
         
        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less than or equals to 50 characters.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less than or equals to 50 characters.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(256, ErrorMessage = "User Name must be less than or equals to 256 characters.")]
        [DisplayName("User Name")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(256, ErrorMessage = "Email must be less than or equals to 256 characters.")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [StringLength(128, ErrorMessage = "Password must be less than or equals to 128 characters.")]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(10, ErrorMessage = "Mobile number must contains 10 digit only.")]
        [MaxLength(10, ErrorMessage = "Mobile number must contains 10 digit only.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid mobile number.")]
        public string PhoneNumber { get; set; } = null!;
    }
}
