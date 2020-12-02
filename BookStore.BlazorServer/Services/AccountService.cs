using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookStore.BlazorServer.Models;

namespace BookStore.BlazorServer.Services
{
    public class AccountService
    {
        public AccountService(HttpClient httpClient, UrlService urlService)
        {
            _httpClient = httpClient;
            _urlService = urlService;
        }

        private readonly HttpClient _httpClient;
        private readonly UrlService _urlService;

        public async Task<ServiceResponse> Register(RegistrationModel model)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _urlService.RegistrationEndpoint)
            {
                Content = JsonContent.Create(model, MediaTypeHeaderValue.Parse("application/json"))
            };

            using var response = await _httpClient.SendAsync(requestMessage);

            return new ServiceResponse
            {
                Succeeded = response.IsSuccessStatusCode,
                Message = response.ReasonPhrase
            };
        }
    }
}