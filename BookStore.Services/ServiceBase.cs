using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Services
{
    public abstract class ServiceBase
    {
        protected virtual void ValidateEntity<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }

            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, validationContext, validationResults, true))
            {
                var builder = new StringBuilder();

                foreach (var result in validationResults)
                {
                    builder.AppendLine($"{string.Join(',', result.MemberNames)} : {string.Join(',', result.ErrorMessage)}");
                }

                throw new ValidationException($"Entity failed validation : {nameof(entity)}. {Environment.NewLine} {builder}");
            }
        }

        protected virtual void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Empty id specified.", nameof(id));
            }
        }
    }
}
