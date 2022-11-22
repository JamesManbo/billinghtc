using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return String.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }
    }
}
