using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace exam
{
    internal class Validators
    {
        private void makeError(string error)
        {
            Console.WriteLine(new Exception($"Validation has been failed: {error}"));

            Environment.Exit(0);
        }

        public bool minLength(string value, int length, string error = null)
        {
            if (error == null) return value.Length >= length;

            if (value.Length < length) makeError(error);

            return true;
        }

        public bool maxLength(string value, int length, string error = null)
        {
            if (error == null) return value.Length <= length;

            if (value.Length > length) makeError(error);

            return true;
        }

        public bool isEmail(string value, string error = null) 
        {
            Regex regex = new Regex(@"[A-z._]{5,}\@[A-z]{2,}\.[A-z]{2,10}");
            Match match = regex.Match(value);

            if (error == null) return match.Success;

            if (!match.Success) makeError(error);

            return true;
        }

        public bool isStrongPassword(string value, string error = null)
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[a-z]).{8,}");
            Match match = regex.Match(value);

            if (error == null) return match.Success;

            if (!match.Success) makeError(error);

            return true;
        }
    }
}
