using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Domain.Entities;

namespace PracticalNineteen.Data.Contexts
{
    public class ApplicationDBContext : IdentityDbContext<UserIdentityModel>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) { }
    }
}
