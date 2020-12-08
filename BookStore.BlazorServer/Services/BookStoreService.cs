using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore.BlazorServer.Models;
using BookStore.Domain.Entities.Dto;
using Newtonsoft.Json;

namespace BookStore.BlazorServer.Services
{
    public class BookStoreService
    {
        public BookStoreService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly MediaTypeHeaderValue _jsonMediaTypeHeaderValue =
            MediaTypeHeaderValue.Parse("application/json");

        public async Task<ServiceResponse<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlService.AuthorsEndpoint);

            try
            {
                var authToken = await _localStorage.GetItemAsync<AuthToken>("AuthToken");

                if (!string.IsNullOrWhiteSpace(authToken?.Token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", authToken.Token);
                }

                var response = await _httpClient.SendAsync(request);
                var serviceResponse = new ServiceResponse<IEnumerable<AuthorDto>>
                {
                    Succeeded = response.IsSuccessStatusCode,
                    Message = response.ReasonPhrase,
                    StatusCode = response.StatusCode
                };

                if (!response.IsSuccessStatusCode)
                {
                    return serviceResponse;
                }

                var content = await response.Content.ReadAsStringAsync();
                serviceResponse.Content = JsonConvert.DeserializeObject<IEnumerable<AuthorDto>>(content);
                return serviceResponse;
            }
            catch (Exception exception)
            {
                return new ServiceResponse<IEnumerable<AuthorDto>>
                {
                    Succeeded = false,
                    Message = exception.Message
                };
            }
        }

        public async Task<ServiceResponse<AuthorDto>> GetAuthorAsync(string id)
        {
            var parsed = Guid.TryParse(id, out var _);

            if (!parsed)
            {
                throw new ArgumentException("Invalid Guid passed.", nameof(id));
            }

            var request = new HttpRequestMessage(HttpMethod.Get, UrlService.GetAuthorEndpoint(id));

            try
            {
                var authToken = await _localStorage.GetItemAsync<AuthToken>("AuthToken");

                if (!string.IsNullOrWhiteSpace(authToken?.Token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", authToken.Token);
                }

                var response = await _httpClient.SendAsync(request);
                var serviceResponse = new ServiceResponse<AuthorDto>
                {
                    Succeeded = response.IsSuccessStatusCode,
                    Message = response.ReasonPhrase,
                    StatusCode = response.StatusCode
                };

                if (!response.IsSuccessStatusCode)
                {
                    return serviceResponse;
                }

                var content = await response.Content.ReadAsStringAsync();
                serviceResponse.Content = JsonConvert.DeserializeObject<AuthorDto>(content);
                return serviceResponse;
            }
            catch (Exception exception)
            {
                return new ServiceResponse<AuthorDto>
                {
                    Succeeded = false,
                    Message = exception.Message
                };
            }
        }
    }
}