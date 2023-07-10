using PracticalNineteen.Domain.DTO;

namespace PracticalNineteen.Domain.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Register new identity user
        /// </summary>
        /// <param name="user">UserModel object</param>
        /// <returns>user identity string</returns>
        Task<bool> RegisterUserAsync(UserModel user);
    }
}
