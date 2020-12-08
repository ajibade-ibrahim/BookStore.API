using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BookStore.BlazorServer.Extensions;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Services;
using BookStore.Domain.Entities.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BookStore.BlazorServer.Pages.Authors
{
    public class AuthorsIndexBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public BookStoreService StoreService { get; set; }

        [Inject]
        public NavigationManager Navigator { get; set; }

        public List<AuthorDto> Authors { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await StoreService.GetAuthors();

                if (response.Succeeded)
                {
                    Authors = response.Content.ToList();
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Navigator.RedirectToLogin();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            JsRuntime.InvokeVoidAsync("InitializeDataTables");
            base.OnAfterRender(firstRender);
        }
    }
}
