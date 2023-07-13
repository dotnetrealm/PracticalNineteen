using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalNineteen.Domain.DTO
{
    public record UserInformation(string UserId, string FirstName, string LastName, string Role);
}

