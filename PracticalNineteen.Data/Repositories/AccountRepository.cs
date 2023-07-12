using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Data.Contexts;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        readonly UserManager<UserIdentity> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly ApplicationDBContext _db;

        public AccountRepository(
            UserManager<UserIdentity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserIdentity> signInManager,
            ApplicationDBContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task<IEnumerable<UserIdentity>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<bool> AddUserToRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist) return false;

            var res = await _userManager.AddToRoleAsync(user, role);
            if (!res.Succeeded) return false;

            return true;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist) return false;

            var res = await _userManager.RemoveFromRoleAsync(user, role);
            if (!res.Succeeded) return false;

            return true;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<bool> RegisterUserAsync(UserIdentity userIdentity)
        {
            var existingUser = await _userManager.FindByEmailAsync(userIdentity.Email);
            if (existingUser != null) return false;

            userIdentity.UserName = userIdentity.Email;
            var res = await _userManager.CreateAsync(userIdentity);
            if (res.Succeeded)
            {
                var roleRes = await _userManager.AddToRoleAsync(userIdentity, "User");
                if (roleRes.Succeeded) return true;
            }
            return false;
        }

        public async Task<bool> CreateRoleAsync(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (roleExist) return false;
            var res = await _roleManager.CreateAsync(new IdentityRole(name));
            return res.Succeeded;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<bool> CheckUserCredsAsync(UserIdentity user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<UserIdentity> GetUserByEmailAsync(string email)
        {
            UserIdentity res = await _userManager.FindByEmailAsync(email);
            return res;
        }

    }
}
