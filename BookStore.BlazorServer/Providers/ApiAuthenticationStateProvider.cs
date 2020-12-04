using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore.BlazorServer.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BookStore.BlazorServer.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ApiAuthenticationStateProvider(
            ILocalStorageService localStorage,
            JwtSecurityTokenHandler tokenHandler,
            ILogger<ApiAuthenticationStateProvider> logger)
        {
            _localStorage = localStorage;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<ApiAuthenticationStateProvider> _logger;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                return new AuthenticationState(await GetClaimsPrincipal());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    $"Error occurred getting AuthenticationState: {exception.Message} | {exception.StackTrace}");
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task<bool> HasValidUser()
        {
            var principal = await GetClaimsPrincipal();
            return principal.Identities.Any();
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            try
            {
                var authToken = await _localStorage.GetItemAsync<AuthToken>("AuthToken");

                if (authToken == null)
                {
                    return new ClaimsPrincipal();
                }

                if (!_tokenHandler.CanReadToken(authToken.Token))
                {
                    return new ClaimsPrincipal();
                }

                var jwtSecurityToken = _tokenHandler.ReadJwtToken(authToken.Token);

                var claims = jwtSecurityToken.Claims.ToList();
                claims.Add(new Claim(ClaimTypes.Name, jwtSecurityToken.Subject));

                return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    $"Error occurred getting ClaimsPrincipal: {exception.Message} | {exception.StackTrace}");
                return new ClaimsPrincipal();
            }
        }

        public void LogUserIn()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void LogUserOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}