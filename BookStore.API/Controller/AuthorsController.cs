using System;
using System.Threading.Tasks;
using BookStore.API.Contracts;
using BookStore.API.Extensions;
using BookStore.Domain.Entities.Dto;
using BookStore.Domain.Exceptions;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : BookStoreControllerBase
    {
        public AuthorsController(IAuthorService authorService, ILoggerService loggerService)
        {
            _authorService = authorService;
            _loggerService = loggerService;
        }

        private readonly IAuthorService _authorService;
        private readonly ILoggerService _loggerService;

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
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
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
            _loggerService.LogInfo("Getting all authors.");

            try
            {
                var authors = await _authorService.GetAllAuthors();
                _loggerService.LogInfo($"Retrieved authors. Count: {authors.Count}.");
                return Ok(authors);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
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
                return BadRequest(GetMessageObject("Invalid identifier."));
            }

            _loggerService.LogInfo($"Getting author with id: {id}");

            try
            {
                var author = await _authorService.GetAuthor(id);
                _loggerService.LogInfo(
                    author == null ? $"Author with id: {id} not found" : $"Author with id: {id} found");

                return author == null ? (IActionResult)NotFound() : Ok(author);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
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
                return BadRequest(GetMessageObject("Invalid identifier."));
            }

            try
            {
                await _authorService.PatchAuthor(id, jsonPatchDocument);
                return NoContent();
            }
            catch (AuthorNotFoundException exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
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
                _loggerService.LogError($"Invalid ModelState. {Environment.NewLine} {GetModelStateErrors()}");
                return ValidationProblem(ModelState);
            }

            try
            {
                var author = await _authorService.CreateAuthor(authorCreationDto);
                _loggerService.LogInfo($"Author with Id: {author.Id} created");

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
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
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
                return BadRequest(GetMessageObject("Invalid identifier."));
            }

            if (!ModelState.IsValid)
            {
                _loggerService.LogError($"Invalid ModelState. {Environment.NewLine} {GetModelStateErrors()}");
                return ValidationProblem(ModelState);
            }

            try
            {
                await _authorService.UpdateAuthor(id, author);
                return NoContent();
            }
            catch (AuthorNotFoundException exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return NotFound($"Author with id: {id} not found.");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error occurred: {exception.GetMessageWithStackTrace()}");
                return InternalServerErrorResult($"Error occurred updating author with id: {id}");
            }
        }
    }
}