using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Helpers
{
    public static class Helpers
    {
        public static string RemoveInvalidNumberInput(string input)
        {
            string cleanString = System.Text.RegularExpressions.Regex.Replace(input, @"[^0-9']", string.Empty).Trim();
            //cleanString = cleanString.Substring(0, Math.Min(cleanString.Length, maxRewardLength));
            return cleanString;
        }
    }
}
