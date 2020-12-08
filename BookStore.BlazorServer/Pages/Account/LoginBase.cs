using System;
using System.Threading.Tasks;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BookStore.BlazorServer.Pages.Account
{
    public class LoginBase : ComponentBase
    {
        public LoginModel Model { get; set; } = new LoginModel();
        public ServiceResponse<string> Response { get; set; } = new ServiceResponse<string>();

        [Inject]
        public AccountService AccountService { get; set; }

        [Inject]
        public NavigationManager Navigator { get; set; }

        [Parameter]
        public string ReturnUrl { get; set; }

        public bool HasErrorMessage
        {
            get => !string.IsNullOrWhiteSpace(Response?.Message) && !Response.Succeeded;
        }

        public async Task Login()
        {
            try
            {
                Response = await AccountService.Login(Model);
                var uri = Navigator.ToAbsoluteUri(Navigator.Uri);

                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var returnUrl))
                {
                    ReturnUrl = $"/{returnUrl}";
                }

                if (Response.Succeeded)
                {
                    Navigator.NavigateTo(ReturnUrl ?? "/");
                }
            }
            catch (Exception exception)
            {
                Response.Succeeded = false;
                Response.Message = exception.Message;
            }
        }
    }
}