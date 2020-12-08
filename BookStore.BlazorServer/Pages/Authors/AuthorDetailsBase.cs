using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BookStore.BlazorServer.Extensions;
using BookStore.BlazorServer.Models;
using BookStore.BlazorServer.Services;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Dto;
using Microsoft.AspNetCore.Components;

namespace BookStore.BlazorServer.Pages.Authors
{
    public class AuthorDetailsBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }
        public AuthorDto Author { get; set; }

        [Inject]
        public BookStoreService StoreService { get; set; }

        [Inject]
        public NavigationManager Navigator { get; set; }

        public ServiceResponse<AuthorDto> Response { get; set; } = new ServiceResponse<AuthorDto>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Response = await StoreService.GetAuthorAsync(Id);

                if (Response.Succeeded)
                {
                    Author = Response.Content;
                }
                else if (Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Navigator.RedirectToLogin();
                }
            }
            catch (Exception exception)
            {
                Response = new ServiceResponse<AuthorDto>
                {
                    Succeeded = false,
                    Message = exception.Message
                };
            }
        }
    }
}
