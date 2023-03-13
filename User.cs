using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace App
{
    internal class User
    {
        public string login;
        public string email;
        public string password;
        public string imageSrc;
    }

    internal class UserJson
    {
        public string login { get; }
        public string email { get; }
        public string passwordHash { get; }
        public string imageSrc { get; }

        public UserJson((string login, string email, string passwordHash, string imageSrc) data)
        {
            login = data.login;
            email = data.email;
            passwordHash = data.passwordHash;
            imageSrc = data.imageSrc;
        }

        public UserJson()
        {
            login = "";
            email = "";
            passwordHash = "";
            imageSrc = "";
        }
    }

    internal class UserModel
    {
        private string dbJsonRoute = "../../data.json";

        private (string value, bool isUnique, bool isRequired) login;
        private (string value, bool isUnique, bool isRequired) email;
        private (string value, bool isUnique, bool isRequired) passwordHash;
        private (string value, bool isUnique, bool isRequired) imageSrc;

        private bool isDB_Existed()
        {
            try { loadDB(); }
            catch { return false; }

            return true;
        }

        private List<UserJson> loadDB()
        {
            string dbJson = File.ReadAllText(dbJsonRoute);

            return JsonConvert.DeserializeObject<List<UserJson>>(dbJson);
        }

        private void saveDB(List<UserJson> users)
        {
            string dbJson = JsonConvert.SerializeObject(users);

            File.WriteAllText(dbJsonRoute, dbJson);
        }

        private void checkRequireness()
        {
            if (login.isRequired && login.value == null) throw new Exception("UserModel, save() error: Login is required.");
            if (email.isRequired && email.value == null) throw new Exception("UserModel, save() error: Email is required.");
            if (passwordHash.isRequired && passwordHash.value == null) throw new Exception("UserModel, save() error: Password hash is required.");
            if (imageSrc.isRequired && imageSrc.value == null) throw new Exception("UserModel, save() error: Image source is required.");
        }

        private void checkUniqueness()
        {
            List<UserJson> existedUsers = loadDB();

            foreach (UserJson existedUser in existedUsers)
            {
                if (login.isUnique)
                {
                    if(existedUser.login == login.value) throw new Exception("UserModel, save() error: Login must be unique.");
                }

                if (email.isUnique)
                {
                    if (existedUser.email == email.value) throw new Exception("UserModel, save() error: Email must be unique.");
                }

                if (passwordHash.isUnique)
                {
                    if (existedUser.passwordHash == passwordHash.value) throw new Exception("UserModel, save() error: Password hash must be unique.");
                }

                if (imageSrc.isUnique)
                {
                    if (existedUser.imageSrc == imageSrc.value) throw new Exception("UserModel, save() error: Image source must be unique.");
                }
            }
        }

        public UserModel(string login, string email, string passwordHash, string imageSrc)
        {
            this.login = (login, false, true);
            this.email = (email, true, true);
            this.passwordHash = (passwordHash, false, true);
            this.imageSrc = (.imageSrc, false, false);

            if(!isDB_Existed()) saveDB(new List<UserJson>());
        }

        public void save()
        {
            try
            {
                checkRequireness();
                checkUniqueness();

                UserJson newUser = new UserJson((login.value, email.value, passwordHash.value, imageSrc.value));

                List<UserJson> existedUsers = loadDB();

                existedUsers.Add(newUser);

                saveDB(existedUsers);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }
        }

        public UserJson findOne(string email)
        {
            List<UserJson> existedUsers = loadDB();

            foreach (UserJson existedUser in existedUsers)
            {
                if (existedUser.email == email) return existedUser;
            }

            return null;
        }

        public void updateOne(UserJson existedUser, UserJson updatedUser)
        {
            try
            {
                if (findOne(existedUser.email) == null)
                {
                    throw new Exception("UserModel, updateOne() error: User wasn't found.");
                }

                new UserModel(updatedUser.login, updatedUser.email, updatedUser.passwordHash, updatedUser.imageSrc).save();

                deleteOne(existedUser);
            }
            catch(Exception err) 
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }
            
        }

        public void deleteOne(UserJson existedUser) 
        {
            try
            {
                if (findOne(existedUser) == null)
                {
                    throw new Exception("UserModel, deleteOne() error: User wasn't found.");
                }

                List<UserJson> existedUsers = loadDB();

                existedUsers.Remove(existedUser);
            } 
            catch(Exception err) 
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }
        }
    }
}
