﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BookStoreControllerBase
    {
        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtConfiguration jwtConfiguration,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfiguration = jwtConfiguration;
            _logger = logger;
        }

        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ILogger<AccountsController> _logger;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            if (userLoginModel == null)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginModel.Username, userLoginModel.Password, true, false);

            if (result?.Succeeded == true)
            {
                var token = await GenerateJwtToken(await _userManager.FindByNameAsync(userLoginModel.Username));
                return Ok(token);
            }

            return Unauthorized(userLoginModel);
        }

        /// <summary>
        ///   Registers a user with the given credentials.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await _userManager.CreateAsync(
                new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Username
                });

            if (result?.Succeeded == true)
            {
                _logger.LogError($"Registration succeeded for user: {model.Username}");
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);

                    if (passwordResult?.Succeeded == true)
                    {
                        return Ok();
                    }

                    _logger.LogError($"Password addition failed for user: {model.Username}");
                    _logger.LogError($"Deleting user: {model.Username}");
                    await _userManager.DeleteAsync(user);

                    var errors = passwordResult?.Errors.Select(x => x.Description) ?? new List<string>();
                    return InternalServerErrorResult(
                        $"Registration failed for user: {model.Username} {Environment.NewLine} {string.Join(';', errors)}");
                }
            }

            result?.Errors.ToList().ForEach(error => _logger.LogError($"{error.Code}: {error.Description}"));

            return InternalServerErrorResult($"Registration failed for user: {model.Username}");
        }

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
    }
}