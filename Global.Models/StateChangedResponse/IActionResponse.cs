using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using FluentValidation.Results;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Global.Models.StateChangedResponse
{
    public interface IActionResponse
    {
        /// <summary>
        /// This holds the list of errors. If the collection is empty, then there were no errors
        /// </summary>
        IImmutableList<ErrorGeneric> Errors { get; }

        /// <summary>
        /// This is true if there are no errors registered
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// On success this returns any message set by GenericServices, or any method that returns a status
        /// If there are errors it contains the message "Failed with NN errors"
        /// </summary>
        string Message { get; }

        /// <summary>
        /// This allows statuses to be combined
        /// </summary>
        /// <param name="nextResponse"></param>
        ActionResponse CombineResponse(IActionResponse nextResponse);

        void AddValidationResults(IEnumerable<ValidationResult> validationResults);
        void AddValidationResults(IEnumerable<ValidationFailure> validationResults);

        /// <summary>
        /// This is a simple method to output all the errors as a single string - null if no errors
        /// Useful for feeding back all the errors in a single exception (also nice in unit testing)
        /// </summary>
        /// <param name="separator"></param>
        /// <returns>a single string with all errors separator by the 'separator' string</returns>
        string GetAllErrors(string separator = "\n");

        void ClearErrors();
        ActionResponse ClearResult();
    }
}