using System;
using System.Runtime.Serialization;

namespace BookStore.Domain.Exceptions
{
    [Serializable]
    public class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException()
        {
        }

        public AuthorNotFoundException(Guid id)
            : base($"Author with id: {id} not found.")
        {
        }

        public AuthorNotFoundException(string message)
            : base(message)
        {
        }

        protected AuthorNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}