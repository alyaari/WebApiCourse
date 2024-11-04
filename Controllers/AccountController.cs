using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication4.DTO;
using WebApplication4.Models;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        ///register
    
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password are not match");
                return BadRequest(ModelState);
            }

            User user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            if(registerDto.IsAdmin)
            await _userManager.AddToRoleAsync(user, "Admin");
           else
                await _userManager.AddToRoleAsync(user, "Visitor");

            return Ok();
        }

        //response type in swagger
        //login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "اليوزر غير موجود تاكد من اسم المستخدم او بريد المستخدم ");
                return BadRequest(ModelState);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
               if(result.IsLockedOut)
                ModelState.AddModelError("IsLockedOut", $"المستخدم موقف بسبب كثرة محاولات الدخول ، الرجاء المحاولة بعد { user.LockoutEnd.GetValueOrDefault().Subtract(DateTime.Now).Minutes } دقيقة ");  
           
               if(result.IsNotAllowed)
                {
                    ModelState.AddModelError("not allowed", $"غير مصرح لك بالدخول يرجى التواصل مع الادارة ");
                }

                return BadRequest(ModelState);
            }

            var claims = new List<Claim>
            {
            new Claim("UserId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName??""),
                new Claim("firstName", user.FirstName??""),
                new Claim("lastName", user.LastName??""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            AuthenticatedResponse response = new AuthenticatedResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredTime = token.ValidTo
            };

            return Ok(response);
            }
    }
}






