using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw3.DTO_s.Requests;
using Cw3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw3.Controllers
{
    [ApiController]//to automatycznie sprawdza zadanie?
    [Authorize(Roles = "employee")]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        public IConfiguration _configuration;

        public EnrollmentsController(IStudentDbService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult AddStudnet(AddStudentRequest request)//odpowiednik EnrollStudent u mnie 
        {
            var response = _service.AddStudent(request);

            return response;

        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            var response = _service.PromoteStudent(request);

            return response;

        }

        [HttpPost("login")]//haslo to ALA do testow
        [AllowAnonymous]
        public IActionResult Login(LoginRequestDto request)
        {
            if (!_service.CheckLoginRequest(request))
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Login),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: "jan",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}