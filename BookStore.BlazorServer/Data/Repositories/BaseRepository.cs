using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookStore.BlazorServer.Data.Contracts;
using Newtonsoft.Json;

namespace BookStore.BlazorServer.Data.Repositories
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        public BaseRepository(HttpClient client)
        {
            _client = client;
        }

        private readonly HttpClient _client;

        public async Task<bool> Create(string url, T obj)
        {
            if (obj == null)
            {
                return true;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
            {
                return false;
            }

            using var request = new HttpRequestMessage(HttpMethod.Delete, url + id);
            using var response = await _client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<T> Get(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<IList<T>> Get(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await _client.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            if (obj == null)
            {
                return false;
            }

            using var request = new HttpRequestMessage(HttpMethod.Put, url + id)
            {
                Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}