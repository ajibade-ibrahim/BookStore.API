using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.BlazorServer.Services
{
    public class AccountService
    {
        private const string AuthToken = "AuthToken";

        public AccountService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider stateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _stateProvider = (ApiAuthenticationStateProvider)stateProvider;
        }

        private readonly HttpClient _httpClient;
        private readonly MediaTypeHeaderValue _jsonMediaTypeHeaderValue =
            MediaTypeHeaderValue.Parse("application/json");
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _stateProvider;

        public async Task<ServiceResponse<string>> Login(LoginModel loginModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UrlService.LoginEndpoint);
            request.Content = JsonContent.Create(loginModel, _jsonMediaTypeHeaderValue);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<string>
                {
                    Succeeded = response.IsSuccessStatusCode,
                    Message = response.ReasonPhrase
                };
            }

            var content = await response.Content.ReadAsStringAsync();
            var authToken = new AuthToken
            {
                Token = content.Trim('"')
            };

            await _localStorage.SetItemAsync(AuthToken, authToken);

            if (await _stateProvider.HasValidUser())
            {
                _stateProvider.LogUserIn();
            }
            else
            {
                return new ServiceResponse<string>
                {
                    Succeeded = false,
                    Message = "Unable to retrieve user data."
                };
            }

            return new ServiceResponse<string>
            {
                Succeeded = true,
                Message = $"Successfully logged in as {loginModel.Username}."
            };
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(AuthToken);
            _stateProvider.LogUserOut();
        }

        public async Task<ServiceResponse<string>> Register(RegistrationModel model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, UrlService.RegistrationEndpoint)
            {
                Content = JsonContent.Create(model, _jsonMediaTypeHeaderValue)
            };

            using var response = await _httpClient.SendAsync(request);

            return new ServiceResponse<string>
            {
                Succeeded = response.IsSuccessStatusCode,
                Message = response.ReasonPhrase
            };
        }
    }
}