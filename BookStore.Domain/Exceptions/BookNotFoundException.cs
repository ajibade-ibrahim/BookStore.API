using System;
using System.Runtime.Serialization;

namespace BookStore.Domain.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException()
        {
        }

        public BookNotFoundException(Guid id)
            : base($"Book with id: {id} not found.")
        {
        }

        public BookNotFoundException(string message)
            : base(message)
        {
        }

        protected BookNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}