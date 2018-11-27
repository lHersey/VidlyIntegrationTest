using System.Linq;
using AutoMapper;
using CleanVidly.Controllers.Resources;
using CleanVidly.Controllers.Users;
using CleanVidly.Core.Entities;

namespace CleanVidly.Mapping
{
    public class EntitiesMapping : Profile
    {
        public EntitiesMapping()
        {
            CreateMap<Category, KeyValuePairResource>();
            CreateMap<Role, KeyValuePairResource>();
            CreateMap<User, UserResource>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(ur => ur.UserRoles.Select(r => r.Role.Description)));
        }

    }
}