using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace App
{
    internal class Bcrypt
    {
        public string GenSalt(int length) { return BCrypt.Net.BCrypt.GenerateSalt(length); }
        public string Hash(string value, string salt) { return BCrypt.Net.BCrypt.HashPassword(value, salt); }
        public bool Verify(string value, string hash) { return BCrypt.Net.BCrypt.Verify(value, hash); }
    }
}
