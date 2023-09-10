using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Product_API.Core.DTO;
using Product_API.Core.Filters;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Product_API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
     private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginModelDTO model)
        {
           
            if (model.Username == "demo" && model.Password == "password")
            {
                // Generate a JWT token
                var token = GenerateJwtToken(model.Username);

                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        private string GenerateJwtToken(string username)
        {

            return
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //
            // var claims = new[]
            // {
            //     new Claim(ClaimTypes.Name, username),
            //     // You can add more claims as needed, e.g., roles, user id, etc.
            // };
            //
            // var token = new JwtSecurityToken(
            //     _configuration["Jwt:Issuer"],
            //     _configuration["Jwt:Audience"],
            //     claims,
            //     expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
            //     signingCredentials: credentials
            // );
            //
            // return new JwtSecurityTokenHandler().WriteToken(token);
        }
}
 
