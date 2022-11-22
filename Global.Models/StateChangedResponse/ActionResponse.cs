using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using FluentValidation.Results;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Global.Models.StateChangedResponse
{
    public class ActionResponse : IActionResponse
    {
        public static ActionResponse Success => new ActionResponse();

        public static ActionResponse SuccessWithResult<TResult>(TResult result)
        {
            var response = new ActionResponse<TResult>();
            response.SetResult(result);
            return response;
        }

        public static ActionResponse Failed(string message, string errorMember = "")
        {
            var actResponse = new ActionResponse();
            actResponse.AddError(message, errorMember);
            return actResponse;
        }

        public static ActionResponse Failed(ErrorGeneric err)
        {
            var actResponse = new ActionResponse();
            actResponse.AddError(err);
            return actResponse;
        }

        internal const string DefaultSuccessMessage = "Thành công";
        private readonly List<ErrorGeneric> _errors = new List<ErrorGeneric>();
        private string _successMessage = DefaultSuccessMessage;

        /// <summary>
        /// This holds the list of ValidationResult errors. If the collection is empty, then there were no errors
        /// </summary>
        public IImmutableList<ErrorGeneric> Errors => _errors.ToImmutableList();

        /// <summary>
        /// This is true if any errors have been reistered 
        /// </summary>
        public bool IsSuccess => !_errors.Any();

        /// <summary>
        /// On success this returns the message as set by the business logic, or the default messages set by the BizRunner
        /// If there are errors it contains the message "Failed with NN errors"
        /// </summary>
        public string Message
        {
            get => IsSuccess
                ? _successMessage
                : $"Thất bại, phát hiện {_errors.Count} lỗi";
            set => _successMessage = value;
        }

        /// <summary>
        /// This adds one error to the Errors collection
        /// </summary>
        /// <param name="errorMessage">The text of the error message</param>
        /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
        public ActionResponse AddError(string errorMessage, params string[] propertyNames)
        {
            if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
            _errors.Add(new ErrorGeneric(new ValidationResult(errorMessage, propertyNames)));
            return this;
        }

        public ActionResponse AddError(ErrorGeneric err)
        {
            _errors.Add(err);
            return this;
        }

        /// <summary>
        /// This adds one ValidationResult to the Errors collection
        /// </summary>
        /// <param name="validationResult"></param>
        public void AddValidationResult(ValidationResult validationResult)
        {
            _errors.Add(new ErrorGeneric(validationResult));
        }

        /// <summary>
        /// This appends a collection of ValidationResults to the Errors collection
        /// </summary>
        /// <param name="validationResults"></param>
        public void AddValidationResults(IEnumerable<ValidationResult> validationResults)
        {
            _errors.AddRange(validationResults.Select(x => new ErrorGeneric(x)));
        }

        /// <summary>
        /// This appends a collection of ValidationFailure to the Errors collection
        /// </summary>
        /// <param name="validationResults"></param>
        public void AddValidationResults(IEnumerable<ValidationFailure> validationResults)
        {
            _errors.AddRange(validationResults.Select(x => new ErrorGeneric(x.ErrorMessage, x.PropertyName)));
        }

        /// <summary>
        /// This allows statuses to be combined. Copies over any errors and replaces the Message if the currect message is null
        /// If you are using Headers then it will combine the headers in any errors in combines
        /// e.g. Status1 with header "MyClass" combines Status2 which has header "MyProp" and status2 has errors.
        /// The result would be error message in status2 would be updates to start with "MyClass>MyProp: This is my error message."
        /// </summary>
        /// <param name="status"></param>
        public ActionResponse CombineResponse(IActionResponse status)
        {
            if (!status.IsSuccess)
            {
                _errors.AddRange(status.Errors);
            }

            if (IsSuccess && status.Message != DefaultSuccessMessage)
                Message = status.Message;

            return this;
        }

        /// <summary>
        /// This is a simple method to output all the errors as a single string - null if no errors
        /// Useful for feeding back all the errors in a single exception (also nice in unit testing)
        /// </summary>
        /// <param name="separator"></param>
        /// <returns>a single string with all errors separator by the 'separator' string</returns>
        public string GetAllErrors(string separator = "\n")
        {
            return _errors.Any()
                ? string.Join(separator, Errors)
                : null;
        }

        public void ClearErrors()
        {
            _errors.Clear();
        }

        public virtual ActionResponse ClearResult()
        {
            //Nothing to do
            return this;
        }
    }
}