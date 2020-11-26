using System;
using System.Threading.Tasks;
using BookStore.API.Extensions;
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
        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        private readonly IBookService _bookService;
        private readonly ILogger _logger;

        /// <summary>
        ///   Gets a book with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid identifier provided.");
            }

            try
            {
                var book = await _bookService.GetBook(id);

                if (book != null)
                {
                    return Ok(book);
                }

                var notFoundMessage = $"Book with id: {id} not found.";
                _logger.LogInformation(notFoundMessage);
                return NotFound(notFoundMessage);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred retrieving book with id: {id}.");
            }
        }

        /// <summary>
        ///   Gets all books.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
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