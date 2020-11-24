using System;
using System.Threading.Tasks;
using BookStore.API.Contracts;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        public AuthorsController(IAuthorService authorService, ILoggerService loggerService)
        {
            _authorService = authorService;
            _loggerService = loggerService;
        }

        private readonly IAuthorService _authorService;
        private readonly ILoggerService _loggerService;

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
                _loggerService.LogError(
                    $"Error occurred: {exception.Message} {Environment.NewLine} {exception.StackTrace}");

                return GetStatusCodeResult(StatusCodes.Status500InternalServerError, "Error occurred retrieving authors.");
            }
        }

        // GET api/<AuthorsController>/5
        /// <summary>
        /// Gets author with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
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
                _loggerService.LogError(
                    $"Error occurred: {exception.Message} {Environment.NewLine} {exception.StackTrace}");

                return GetStatusCodeResult(
                    StatusCodes.Status500InternalServerError,
                    $"Error occurred retrieving author with id: {id}.");
            }
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        private ObjectResult GetStatusCodeResult(int statusCode, string message)
        {
            return StatusCode(
                statusCode,
                new
                {
                    message
                });
        }
    }
}