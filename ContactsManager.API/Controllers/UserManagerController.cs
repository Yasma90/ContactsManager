using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Domain.Dto;
using ContactsManager.Domain.Models;
using ContactsManager.Persistence;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ContactsManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;


        public UserManagerController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public ActionResult<List<User>> GetUsersAsync() => _context.Users.AsQueryable().OrderBy(u => u.Email).ToList();

        [HttpPost("add-admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult<List<User>>> AddAdminAsync([FromBody]  string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddClaimAsync(user, new Claim("Role", "Admin"));

            return NoContent();
        }

        [HttpPost("remove-admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult<List<User>>> RemoveAdminAsync([FromBody] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveClaimAsync(user, new Claim("Role", "Admin"));

            return NoContent();
        }

        [HttpPost("create-user")]
        [Authorize]
        public async Task<ActionResult<AuthenticationResponse>> CreateAsync([FromBody] UserRequest request)
        {
            var user = new User { UserName = request.UserName, Email = request.Email, Password = request.Password };
            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded ? await CreateToken(request) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password,
                isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded ? await CreateToken(request) : BadRequest("Incorrect Login.");
        }


        private async Task<AuthenticationResponse> CreateToken(UserRequest request)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email,request.Email)
            };

            //Add claims into db
            var user = await _userManager.FindByIdAsync(request.Email);
            var claimsDb = await _userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDb);

            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);

            var exp = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: exp, signingCredentials: creds);

            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = exp
            };
        }
    }
}
