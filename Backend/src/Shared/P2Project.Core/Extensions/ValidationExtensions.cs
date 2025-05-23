﻿using FluentValidation.Results;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static ErrorList ToErrorList(
            this ValidationResult validationResult)
        {
            var validationErrors = validationResult.Errors;

            var errors = from validationError in validationErrors
                         let errorMessage = validationError.ErrorMessage
                         let error = Error.Deserialize(errorMessage)
                         select Error.Validation(
                             error.Code, 
                             error.Message, 
                             validationError.PropertyName);

            return errors.ToList();
        }
    }
}
