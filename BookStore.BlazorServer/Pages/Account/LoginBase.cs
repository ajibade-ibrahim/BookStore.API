using System;
using System.Threading.Tasks;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Services;
using Microsoft.AspNetCore.Components;

namespace BookStore.BlazorServer.Pages.Account
{
    public class LoginBase : ComponentBase
    {
        public LoginModel Model { get; set; } = new LoginModel();
        public ServiceResponse Response { get; set; } = new ServiceResponse();

        [Inject]
        public AccountService AccountService { get; set; }

        [Inject]
        public NavigationManager Navigator { get; set; }

        public bool HasErrorMessage
        {
            get => !string.IsNullOrWhiteSpace(Response?.Message) && !Response.Succeeded;
        }

        public async Task Login()
        {
            try
            {
                Response = await AccountService.Login(Model);

                if (Response.Succeeded)
                {
                    Navigator.NavigateTo("/");
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