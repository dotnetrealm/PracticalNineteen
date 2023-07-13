using Microsoft.AspNetCore.Identity;
using PracticalNineteen.Domain.Entities;

namespace PracticalNineteen.Domain.Interfaces
{
    public interface IAccountRepository
    {

        /// <summary>
        /// Return list of users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserIdentity>> GetAllUsersAsync();

        /// <summary>
        /// Add user to role
        /// </summary>
        /// <returns></returns>
        Task<bool> AddUserToRoleAsync(string email, string role);

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="email">email address</param>
        /// <param name="role">role name</param>
        /// <returns></returns>
        Task<bool> RemoveUserFromRoleAsync(string email, string role);

        /// <summary>
        /// Return all roles assigned to user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetUserRolesAsync(string email);

        /// <summary>
        /// Register new identity user
        /// </summary>
        /// <param name="user">UserModel object</param>
        /// <returns>true if created successfully, else false</returns>
        Task<bool> RegisterUserAsync(UserIdentity userIdentity, string password);

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="roleModel">RoleModel object</param>
        /// <returns>true if created successfully, else false</returns>
        Task<bool> CreateRoleAsync(string name);

        /// <summary>
        /// Return list of available identity roles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();

        /// <summary>
        /// Check given information of user is valid or not
        /// </summary>
        /// <param name="user">User identity object</param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckUserCredsAsync(UserIdentity user, string password);

        /// <summary>
        /// Return user information by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserIdentity> GetUserByEmailAsync(string email);

        /// <summary>
        /// Delete user 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(string email);
    }
}
