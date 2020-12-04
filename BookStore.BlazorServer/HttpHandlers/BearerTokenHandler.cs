using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore.BlazorServer.Models;
using Microsoft.Extensions.Logging;

namespace BookStore.BlazorServer.HttpHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        public BearerTokenHandler(ILocalStorageService localStorage, ILogger<BearerTokenHandler> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }

        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<BearerTokenHandler> _logger;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var authToken = await _localStorage.GetItemAsync<AuthToken>("AuthToken");

                if (!string.IsNullOrWhiteSpace(authToken?.Token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", authToken.Token);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.Message}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}