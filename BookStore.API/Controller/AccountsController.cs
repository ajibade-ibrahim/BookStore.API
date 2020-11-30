using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtConfiguration jwtConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfiguration = jwtConfiguration;
        }

        private readonly JwtConfiguration _jwtConfiguration;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var token = new JwtSecurityToken(
                _jwtConfiguration.Issuer,
                _jwtConfiguration.Issuer,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(1),
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Retrieves an authentication token.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(userInfo.Username, userInfo.Password, true, false);

            if (result?.Succeeded == true)
            {
                var token = await GenerateJwtToken(await _userManager.FindByNameAsync(userInfo.Username));
                return Ok(token);
            }

            return Unauthorized(userInfo);
        }
    }
}