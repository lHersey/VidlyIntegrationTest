using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CleanVidly.Controllers.Resources;
using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanVidly.Controllers.Roles
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public RolesController(IMapper mapper, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<KeyValuePairResource>> GetAllRoles()
        {
            var roles = await roleRepository.GetFindAllAsync();
            return mapper.Map<IEnumerable<KeyValuePairResource>>(roles);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            var role = await roleRepository.FindUniqueAsync(c => c.Id == roleId);

            if (role is null) return NotFound("Role not found");

            return Ok(mapper.Map<KeyValuePairResource>(role));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddNewRole(SaveRoleResource saveRoleResource)
        {
            var role = mapper.Map<Role>(saveRoleResource);

            await roleRepository.AddAsync(role);
            await unitOfWork.SaveAsync();

            var categoryResource = mapper.Map<KeyValuePairResource>(role);
            return Ok(categoryResource);
        }

        [Authorize]
        [HttpPut("{roleId}")]
        public async Task<ActionResult> UpdateExistintRole(int roleId, SaveRoleResource saveRoleResource)
        {
            var role = await roleRepository.FindUniqueAsync(c => c.Id == roleId);
            if (role is null) return NotFound("Role not found");

            mapper.Map<SaveRoleResource, Core.Entities.Role>(saveRoleResource, role);

            await unitOfWork.SaveAsync();

            var roleResource = mapper.Map<KeyValuePairResource>(role);
            return Ok(roleResource);
        }

        [Authorize]
        [HttpDelete("{roleId}")]
        public async Task<ActionResult> DeleteExistingRole(int roleId)
        {
            var category = await roleRepository.FindUniqueAsync(c => c.Id == roleId);
            if (category is null) return NotFound("Category not found");

            roleRepository.Remove(category);
            await unitOfWork.SaveAsync();

            var categoryResource = mapper.Map<KeyValuePairResource>(category);
            return Ok(categoryResource);
        }
    }
}