using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App
{
    internal class UserJson
    {
        public string login;
        public string email;
        public string passwordHash;
        public string imageSrc;

        public UserJson(string login, string email, string passwordHash, string imageSrc)
        {
            this.login = login;
            this.email = email;
            this.passwordHash = passwordHash;
            this.imageSrc = imageSrc;
        }   
    }

    internal class UserModel
    {
        UserJson userJson;
        private string db;

        private (string value, bool isUnique, bool isRequired) login;
        private (string value, bool isUnique, bool isRequired) email;
        private (string value, bool isUnique, bool isRequired) passwordHash;
        private (string value, bool isUnique, bool isRequired) imageSrc;

        private void requiredCheckings()
        {
            if (login.isRequired == true && login.value == null) throw new Exception("UserModel, save() error: Login is required.");
            if (email.isRequired == true && email.value == null) throw new Exception("UserModel, save() error: Email is required.");
            if (passwordHash.isRequired == true && passwordHash.value == null) throw new Exception("UserModel, save() error: Password hash is required.");
            if (imageSrc.isRequired == true && imageSrc.value == null) throw new Exception("UserModel, save() error: Image source is required.");
        }

        private void uniqueCheckings() { /*soon*/ }

        public UserModel(string login, string email, string passwordHash, string imageSrc)
        {
            this.login = (login, false, true);
            this.email = (email, true, true);
            this.passwordHash = (passwordHash, false, true);
            this.imageSrc = (imageSrc, false, false);

            this.userJson = new UserJson(login, email, passwordHash, imageSrc);
        }

        public void save()
        {
            try
            {
                requiredCheckings();
                uniqueCheckings();

                var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
                db = JsonSerializer.Serialize<UserJson>(this.userJson, jsonOptions);

                Console.WriteLine(db);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                Environment.Exit(0);
            }
        }
    }
}
