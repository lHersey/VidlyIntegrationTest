using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanVidly.Controllers.Users;
using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using CleanVidly.Extensions;
using CleanVidly.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanVidly.Controllers.Auth
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly JwtAuthentication jwtAuthentication;

        public AuthController(JwtAuthentication jwtAuthentication, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.jwtAuthentication = jwtAuthentication;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthResource authResource)
        {
            var user = await userRepository.GetWithRoles(u => u.Email == authResource.Email);
            if (user is null) return this.BadRequest("Authorization", "Invalid email or password");

            var validPassword = Hashing.VerifyHash(authResource.Password, user.Salt, user.Password);
            if (!validPassword) return this.BadRequest("Authorization", "Invalid email or password");

            var token = jwtAuthentication.GenerateToken(user);
            return Ok(new { token });
        }


    }
}