using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ESTUDO.API.Data;
using ESTUDO.API.DTO;
using ESTUDO.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ESTUDO.API.Controllers
{
    /// <summary>
    /// Controller responsible for registering new users and logging in existent ones.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _database;
        /// <summary>
        /// Constructor for the account controller, it injects the context, SignInManager, UserManager and IConfiguration.
        /// </summary>
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration config, ApplicationDbContext database)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _database = database;
        }
        /// <summary>
        /// New user registration
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if(ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByEmailAsync(model.Email);
                if(userFromDb == null)
                {
                    try
                    {
                        var role = _database.Roles.First(role => role.NormalizedName == model.Role.ToUpper());
                        User newUser = new User();
                        newUser.UserName = model.Email;
                        newUser.Email = model.Email;
                        newUser.EmailConfirmed = true;
                        newUser.GivenName = model.GivenName;
                        newUser.Surname = model.Surname;

                        IdentityResult result = _userManager.CreateAsync(newUser, model.Password).Result;

                        await _userManager.AddToRoleAsync(newUser, model.Role);
                        return Created("", new{userId = newUser.Id, userName = model.Email, role = model.Role});
                    }
                    catch (System.Exception)
                    {
                        Response.StatusCode = 404;
                        return new ObjectResult("Role not found");
                    }
                }
                return BadRequest("Email already registered");
            }
            return BadRequest();
        }
        /// <summary>
        /// Existent user login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if(ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByEmailAsync(model.Username);
                if(userFromDb != null)
                {
                    var passwordCheck = await _signInManager.CheckPasswordSignInAsync(userFromDb, model.Password, false);
                    if(passwordCheck.Succeeded)
                    {
                        var token = Generate(userFromDb);
                        return Ok(token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            return BadRequest();
        }
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            string roleId = _database.UserRoles.First(role => role.UserId == user.Id).RoleId;

            var claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, user.GivenName));
            claims.Add(new Claim(ClaimTypes.Surname, user.Surname));
            claims.Add(new Claim(ClaimTypes.Role, _database.Roles.First(role => role.Id == roleId).ToString()));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}