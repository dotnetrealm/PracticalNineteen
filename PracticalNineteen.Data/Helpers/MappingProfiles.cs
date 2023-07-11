using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Models;

namespace PracticalNineteen.Data.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserIdentity, UserRegistrationModel>().ReverseMap();
            CreateMap<IdentityRole, RoleModel>().ReverseMap();
            CreateMap<Student, StudentModel>().ReverseMap();
        }
    }
}
