namespace App
{
    internal class Bcrypt
    {
        public string GenSalt(int length) { return BCrypt.Net.BCrypt.GenerateSalt(length); }
        public string Hash(string value, string salt) { return BCrypt.Net.BCrypt.HashPassword(value, salt); }
        public bool Verify(string value, string hash) { return BCrypt.Net.BCrypt.Verify(value, hash); }
    }
}
