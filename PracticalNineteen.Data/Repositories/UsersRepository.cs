using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Data.Repositories
{
    public class UsersRepository : IUserRepository
    {
        readonly UserManager<UserIdentityModel> _userManager;
        readonly IMapper _mapper;

        public UsersRepository(UserManager<UserIdentityModel> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        
    }
}
