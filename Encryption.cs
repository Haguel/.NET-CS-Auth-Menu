using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace App
{
    internal class Encryption
    {
        public string genSalt(int length) { return BCrypt.Net.BCrypt.GenerateSalt(length); }
        public string hash(string value, string salt) { return BCrypt.Net.BCrypt.HashPassword(value, salt); }
        public bool verify(string value, string hash) { return BCrypt.Net.BCrypt.Verify(value, hash); }
    }
}
