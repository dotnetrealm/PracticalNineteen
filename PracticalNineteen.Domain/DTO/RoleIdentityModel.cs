using System.ComponentModel.DataAnnotations;

namespace PracticalNineteen.Domain.DTO
{
    public class RoleModel
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
