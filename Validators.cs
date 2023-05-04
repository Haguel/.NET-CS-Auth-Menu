using System;
using System.Text.RegularExpressions;

namespace exam
{
    internal class Validators
    {
        private void MakeError(string error)
        {
            throw new Exception($"Validation has been failed: {error}");
        }

        public bool MinLength(string value, int length, string error = null)
        {
            if (error == null) return value.Length >= length;

            if (value.Length < length) MakeError(error);

            return true;
        }

        public bool MaxLength(string value, int length, string error = null)
        {
            if (error == null) return value.Length <= length;

            if (value.Length > length) MakeError(error);

            return true;
        }

        public bool IsEmail(string value, string error = null) 
        {
            Regex regex = new Regex(@"[A-z._]{5,}\@[A-z]{2,}\.[A-z]{2,10}");
            Match match = regex.Match(value);

            if (error == null) return match.Success;

            if (!match.Success) MakeError(error);

            return true;
        }

        public bool IsStrongPassword(string value, string error = null)
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[a-z]).{8,}");
            Match match = regex.Match(value);

            if (error == null) return match.Success;

            if (!match.Success) MakeError(error);

            return true;
        }
    }
}
