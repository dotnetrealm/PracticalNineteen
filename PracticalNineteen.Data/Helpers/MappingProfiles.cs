using AutoMapper;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;

namespace PracticalNineteen.Data.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserIdentityModel, UserModel>().ReverseMap();
        }
    }
}
