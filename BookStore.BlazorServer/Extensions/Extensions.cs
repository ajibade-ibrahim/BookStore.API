using Microsoft.AspNetCore.Components;

namespace BookStore.BlazorServer.Extensions
{
    public static class Extensions
    {
        public static void RedirectToLogin(this NavigationManager navigationManager)
        {
            var uri = $"/login?returnUrl={navigationManager.Uri.Replace(navigationManager.BaseUri, string.Empty)}";
            navigationManager.NavigateTo(uri);
        }
    }
}