using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Data.Context;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Models;

namespace PracticalNineteen.Data.Contexts
{
    public class ApplicationDBContext : IdentityDbContext<UserIdentity>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedStudents();
        }
    }
}
