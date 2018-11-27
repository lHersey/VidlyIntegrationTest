using System;
using System.Threading.Tasks;
using AutoMapper;
using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using CleanVidly.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CleanVidly.Extensions;
using Microsoft.Extensions.Configuration;

namespace CleanVidly.Controllers.Users
{
    [Route("/api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly JwtAuthentication jwtAuthentication;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitofWork;

        public UsersController(IMapper mapper, IConfiguration configuration, IUserRepository userRepository, IUnitOfWork unitofWork)
        {
            this.unitofWork = unitofWork;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.jwtAuthentication = new JwtAuthentication(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(SaveUserResource saveUserResource)
        {
            var userAlreadyExist = await userRepository.ExistAsync(u => u.Email == saveUserResource.Email);
            if (userAlreadyExist) return this.BadRequest("Email", "Email already registered");

            var user = mapper.Map<User>(saveUserResource);

            user.JoinDate = DateTime.UtcNow;
            user.Salt = Hashing.GenerateSalt();
            user.Password = Hashing.CreateHash(user.Salt, saveUserResource.Password);

            await userRepository.AddAsync(user);
            await unitofWork.SaveAsync();

            user = await userRepository.GetWithRoles(u => u.Email == user.Email);

            var token = jwtAuthentication.GenerateToken(user);
            Response.Headers.Add("x-auth-token", token);

            var userResource = mapper.Map<User, UserResource>(user);
            return Ok(userResource);
        }
    }

}