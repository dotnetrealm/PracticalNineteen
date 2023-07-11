using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Data.Repositories
{
    public class UsersRepository : IUserRepository
    {
        readonly UserManager<UserIdentity> _userManager;
        readonly IMapper _mapper;

        public UsersRepository(UserManager<UserIdentity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


    }
}
