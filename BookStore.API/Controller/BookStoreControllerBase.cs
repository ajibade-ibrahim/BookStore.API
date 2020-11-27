using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStore.API.Controller
{
    public class BookStoreControllerBase : ControllerBase
    {
        protected const string InvalidIdentifier = "Invalid identifier.";

        protected void LogModelStateErrors(ILogger logger)
        {
            logger.LogError($"ModelState Errors: . {Environment.NewLine} {GetModelStateErrors()}");
        }

        protected static object GetMessageObject(string message)
        {
            return new
            {
                message
            };
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var apiBehaviorOptions = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>()
                .Value;
            var result = apiBehaviorOptions.InvalidModelStateResponseFactory(ControllerContext);
            return (ActionResult)result;
        }

        protected string GetModelStateErrors()
        {
            var builder = new StringBuilder();
            var errorKeys = ModelState.Keys.Where(key => ModelState[key].Errors.Any()).ToList();
            builder.Append($"Error count: {errorKeys.Count}");

            foreach (var key in errorKeys)
            {
                builder.AppendLine($"{key}: {string.Join(';', ModelState[key].Errors.Select(x => x.ErrorMessage))}");
            }

            return builder.ToString();
        }

        protected ObjectResult GetStatusCodeResult(int statusCode, string message)
        {
            return StatusCode(statusCode, GetMessageObject(message));
        }

        protected ObjectResult InternalServerErrorResult(string message)
        {
            return GetStatusCodeResult(StatusCodes.Status500InternalServerError, message);
        }
    }
}