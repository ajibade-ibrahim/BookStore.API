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
    }
}