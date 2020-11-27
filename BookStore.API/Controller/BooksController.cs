using System;
using System.Threading.Tasks;
using BookStore.API.Extensions;
using BookStore.Domain.Entities.Dto;
using BookStore.Domain.Exceptions;
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
                var book = await _bookService.GetBookAsync(id);

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
                return Ok(await _bookService.GetAllBooksAsync());
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred retrieving books.");
            }
        }

        /// <summary>
        /// Creates a book with the provided information.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreationDto book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            if (book.AuthorId == Guid.Empty)
            {
                return BadRequest("Invalid author identifier");
            }

            if (!ModelState.IsValid)
            {
                LogModelStateErrors(_logger);
                return ValidationProblem(ModelState);
            }

            try
            {
                var bookDto = await _bookService.CreateBookAsync(book);
                return CreatedAtAction(
                    "GetBook",
                    new
                    {
                        id = bookDto.Id
                    },
                    bookDto);
            }
            catch (AuthorNotFoundException exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Author with id:{book.AuthorId} not found.");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred retrieving books.");
            }
        }

        /// <summary>
        /// Updates a book with the provided information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookUpdateDto book)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(InvalidIdentifier);
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                await _bookService.UpdateBookAsync(id, book);
                return NoContent();
            }
            catch (BookNotFoundException exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Book with id:{id} not found.");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred updating book with id: {id}.");
            }
        }
    }
}