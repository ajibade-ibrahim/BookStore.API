using System;
using System.Threading.Tasks;
using BookStore.API.Extensions;
using BookStore.Domain.Entities.Dto;
using BookStore.Domain.Exceptions;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : BookStoreControllerBase
    {
        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        private readonly IAuthorService _authorService;
        private readonly ILogger _logger;

        // DELETE api/<AuthorsController>/5
        /// <summary>
        ///   Deletes the author with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(GetMessageObject("Invalid identifier"));
            }

            try
            {
                await _authorService.DeleteAuthor(id);
                return NoContent();
            }
            catch (AuthorNotFoundException exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred deleting author with id: {id}.");
            }
        }

        // GET: api/<AuthorsController>
        /// <summary>
        ///   Gets all existing authors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all authors.");

            try
            {
                var authors = await _authorService.GetAllAuthors();
                _logger.LogInformation($"Retrieved authors. Count: {authors.Count}.");
                return Ok(authors);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred retrieving authors.");
            }
        }

        // GET api/<AuthorsController>/5
        /// <summary>
        ///   Gets author with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(GetMessageObject(InvalidIdentifier));
            }

            _logger.LogInformation($"Getting author with id: {id}");

            try
            {
                var author = await _authorService.GetAuthor(id);
                _logger.LogInformation(
                    author == null ? $"Author with id: {id} not found" : $"Author with id: {id} found");

                return author == null ? (IActionResult)NotFound() : Ok(author);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred retrieving author with id: {id}.");
            }
        }

        /// <summary>
        ///   Updates an author with provided information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<AuthorUpdateDto> jsonPatchDocument)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(GetMessageObject(InvalidIdentifier));
            }

            try
            {
                await _authorService.PatchAuthor(id, jsonPatchDocument);
                return NoContent();
            }
            catch (AuthorNotFoundException exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred updating author with id: {id}");
            }
        }

        // POST api/<AuthorsController>
        /// <summary>
        ///   Creates an author with the provided information.
        /// </summary>
        /// <param name="authorCreationDto"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] AuthorCreationDto authorCreationDto)
        {
            if (authorCreationDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                LogModelStateErrors(_logger);
                return ValidationProblem(ModelState);
            }

            try
            {
                var author = await _authorService.CreateAuthor(authorCreationDto);
                _logger.LogInformation($"Author with Id: {author.Id} created");

                return CreatedAtAction(
                    "Get",
                    new
                    {
                        author.Id
                    },
                    author);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult("Error occurred creating Author.");
            }
        }

        // PUT api/<AuthorsController>/5
        /// <summary>
        ///   Updates an author with the provided information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] AuthorUpdateDto author)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(GetMessageObject(InvalidIdentifier));
            }

            if (!ModelState.IsValid)
            {
                LogModelStateErrors(_logger);
                return ValidationProblem(ModelState);
            }

            try
            {
                await _authorService.UpdateAuthor(id, author);
                return NoContent();
            }
            catch (AuthorNotFoundException exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred updating author with id: {id}");
            }
        }
    }
}