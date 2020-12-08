using System;

namespace BookStore.BlazorServer.Services
{
    public static class UrlService
    {
        public static string AuthorsEndpoint
        {
            get => "Authors";
        }

        public static string BaseUrl { get; set; }

        public static string BooksEndpoint
        {
            get => "Books";
        }

        public static string LoginEndpoint
        {
            get => "accounts/login";
        }

        public static string RegistrationEndpoint
        {
            get => "accounts/register";
        }

        public static string GetAuthorEndpoint(string id)
        {
            return $"Authors/{id}";
        }

        public static string GetAuthorEndpoint(Guid id)
        {
            return id == Guid.Empty ? throw new ArgumentException(nameof(id)) : GetAuthorEndpoint(id.ToString());
        }
    }
}