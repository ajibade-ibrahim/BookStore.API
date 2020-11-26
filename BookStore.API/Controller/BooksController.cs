using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Contracts;
using BookStore.API.Extensions;
using BookStore.Domain.Entities;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : BookStoreControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all books.");

            try
            {
                return Ok(await _bookService.GetAllBooks());
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred retrieving books.");
            }
        }
    }
}
