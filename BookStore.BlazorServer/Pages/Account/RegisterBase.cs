using System.Threading.Tasks;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Services;
using Microsoft.AspNetCore.Components;

namespace BookStore.BlazorServer.Pages.Account
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        public AccountService AccountService { get; set; }

        public RegistrationModel Model { get; set; } = new RegistrationModel();

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ServiceResponse Response { get; set; } = new ServiceResponse();

        public bool HasErrorMessage
        {
            get => !string.IsNullOrWhiteSpace(Response?.Message) && !Response.Succeeded;
        }

        public async Task Register()
        {
            Response = await AccountService.Register(Model);

            if (Response.Succeeded)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}