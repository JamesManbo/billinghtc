using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Global.Models
{
    public struct ErrorGeneric
    {

        /// <summary>
        /// This ctor will create an ErrorGeneric
        /// </summary>
        /// <param name="error"></param>
        public ErrorGeneric(ValidationResult error) : this()
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            MemberName = error.MemberNames?.FirstOrDefault();
            ErrorMessage = error.ErrorMessage;
        }
        public ErrorGeneric(string errorMessage, string memberName = null) : this()
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            MemberName = memberName;
        }

        /// <summary>
        /// A Member Name
        /// </summary>
        public string MemberName { get; private set; }

        /// <summary>
        /// This is the error provided
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// A human-readable error display
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{(string.IsNullOrEmpty(MemberName) ? MemberName + ":" : string.Empty)}{ErrorMessage}";
        }

    }
}
