using System;

namespace BookStore.API.Extensions
{
    public static class HelperExtensions
    {
        public static string GetMessageWithStackTrace(this Exception exception)
        {
            return $"{exception.Message}. {Environment.NewLine} {exception.StackTrace}";
        }
    }
}