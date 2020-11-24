﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.API.Contracts;
using BookStore.API.Extensions;
using BookStore.Domain.Entities.Dto;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var apiBehaviorOptions = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>()
                .Value;
            var result = apiBehaviorOptions.InvalidModelStateResponseFactory(ControllerContext);
            return (ActionResult)result;
        }

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
                    $"Error occurred: {exception.GetMessageWithStackTrace()}");

                return GetStatusCodeResult(
                    StatusCodes.Status500InternalServerError,
                    "Error occurred retrieving authors.");
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
                    $"Error occurred: {exception.GetMessageWithStackTrace()}");

                return GetStatusCodeResult(
                    StatusCodes.Status500InternalServerError,
                    $"Error occurred retrieving author with id: {id}.");
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
                _loggerService.LogError($"Invalid ModelState. {Environment.NewLine} {LogModelStateErrors()}");
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
                return GetStatusCodeResult(StatusCodes.Status500InternalServerError, "Error occurred creating Author.");
            }
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

        private string LogModelStateErrors()
        {
            var builder = new StringBuilder();
            var errorKeys = ModelState.Keys.Where(key => ModelState[key].Errors.Any());

            foreach (var key in errorKeys)
            {
                builder.AppendLine($"{key}: {string.Join(';', ModelState[key].Errors.Select(x => x.ErrorMessage))}");
            }

            return builder.ToString();
        }
    }
}