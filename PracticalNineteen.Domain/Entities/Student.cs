using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticalNineteen.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name must contains less than 50 letters.")]
        [DisplayName("First name")]
        public string FirstName { get; set; } = null!;

        [DisplayFormat(NullDisplayText = "-")]
        [DisplayName("Middle name")]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name must contains less than 50 letters.")]
        [DisplayName("Last name")]
        public string LastName { get; set; } = null!;

        [Required]
        [DisplayName("Birth date (MM-dd-yyyy)")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; } = new DateTime(2002, 01, 01).Date;

        [Required]
        public char Gender { get; set; } = 'M';

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10)]
        [DisplayName("Mobile number")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid mobile number.")]
        [Column("varchar(10)")]
        public string MobileNumber { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Last name must contains less than 100 letters")]
        [DisplayFormat(NullDisplayText = "-")]
        public string? Address { get; set; }
    }
}
