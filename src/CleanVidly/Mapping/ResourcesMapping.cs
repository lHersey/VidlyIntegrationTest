using System.Linq;
using AutoMapper;
using CleanVidly.Controllers.Categories;
using CleanVidly.Controllers.Roles;
using CleanVidly.Controllers.Users;
using CleanVidly.Core.Entities;

namespace CleanVidly.Mapping
{
    public class ResourcesMapping : Profile
    {
        public ResourcesMapping()
        {
            CreateMap<SaveCategoryResource, Category>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Description, opt => opt.MapFrom(sc => sc.Description));

            CreateMap<SaveRoleResource, Role>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Description, opt => opt.MapFrom(sc => sc.Description));

            CreateMap<SaveUserResource, User>()
                .ForMember(u => u.Password, opt => opt.Ignore())
                .ForMember(u => u.Salt, opt => opt.Ignore())
                .ForMember(u => u.JoinDate, opt => opt.Ignore())
                .AfterMap((ur, u) =>
                {
                    //Remove unused role
                    var removedRoles = u.UserRoles.Where(f => !ur.Roles.Contains(f.RoleId));
                    foreach (var l in removedRoles)
                        u.UserRoles.Remove(l);

                    //Add new roles
                    var addRoles = ur.Roles.Where(id => !u.UserRoles.Any(f => f.RoleId == id)).Select(id => new UserRole { RoleId = id });
                    foreach (var f in addRoles)
                        u.UserRoles.Add(f);
                });
        }
    }
}
