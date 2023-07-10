using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        readonly UserManager<UserIdentityModel> _userManager;
        readonly IMapper _mapper;

        public AccountRepository(UserManager<UserIdentityModel> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<bool> RegisterUserAsync(UserModel user)
        {
            var userIdentity = _mapper.Map<UserIdentityModel>(user);
            var res = await _userManager.CreateAsync(userIdentity, user.Password);
            return res.Succeeded;
        }
    }
}
