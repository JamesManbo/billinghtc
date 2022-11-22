using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Global.Models.StateChangedResponse
{
    public interface IActionResponse<out T> : IActionResponse
    {
        /// <summary>
        /// Holds the value set using SetSuccessWithResult
        /// </summary>
        T Result { get; }
    }
}
