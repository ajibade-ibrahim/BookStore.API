namespace BookStore.BlazorServer.Services
{
    public class UrlService
    {
        public string AuthorsEndpoint
        {
            get => $"{BaseUrl}/Authors";
        }

        public string BaseUrl { get; set; }

        public string BooksEndpoint
        {
            get => $"{BaseUrl}/Books";
        }

        public string LoginEndpoint
        {
            get => $"{BaseUrl}/login";
        }

        public string RegistrationEndpoint
        {
            get => $"{BaseUrl}/register";
        }

        public UrlService(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}