﻿using System;
using System.Collections.Generic;

namespace App
{
    internal class UserSettings
    {
        public readonly (bool isUnique, bool isRequired) login = (false, true);
        public readonly (bool isUnique, bool isRequired) email = (true, true);
        public readonly (bool isUnique, bool isRequired) passwordHash = (false, true);
        public readonly (bool isUnique, bool isRequired) imageSrc = (false, false);
    }

    internal class BaseUser
    {
        public string login;
        public string email;
        public string password;
        public string imageSrc;
    }

    internal class CompletedUser
    {
        public string login { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string imageSrc { get; set; }

        public CompletedUser(string login, string email, string passwordHash, string imageSrc)
        {
            this.login = login;
            this.email = email;
            this.passwordHash = passwordHash;
            this.imageSrc = imageSrc;
        }

        public CompletedUser()
        {
            login = "";
            email = "";
            passwordHash = "";
            imageSrc = "";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CompletedUser);
        }

        public bool Equals(CompletedUser obj)
        {
            return obj != null && obj.email == this.email && obj.login == this.login && obj.imageSrc == this.imageSrc && obj.passwordHash == this.passwordHash;
        }

    }

    internal class UserModel
    {
        Database database;
        private (string value, bool isUnique, bool isRequired) login;
        private (string value, bool isUnique, bool isRequired) email;
        private (string value, bool isUnique, bool isRequired) passwordHash;
        private (string value, bool isUnique, bool isRequired) imageSrc;

        private void CheckRequireness()
        {
            if (login.isRequired && login.value == null) throw new Exception("UserModel, save() error: Login is required.");
            if (email.isRequired && email.value == null) throw new Exception("UserModel, save() error: Email is required.");
            if (passwordHash.isRequired && passwordHash.value == null) throw new Exception("UserModel, save() error: Password hash is required.");
            if (imageSrc.isRequired && imageSrc.value == null) throw new Exception("UserModel, save() error: Image source is required.");
        }

        private void CheckUniqueness()
        {
            List<CompletedUser> existedUsers = database.LoadUsers();

            foreach (CompletedUser existedUser in existedUsers)
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

        public UserModel(CompletedUser userCompleted)
        {
            UserSettings userSettigns = new UserSettings();

            login = (userCompleted.login, userSettigns.login.isUnique, userSettigns.login.isRequired);
            email = (userCompleted.email, userSettigns.email.isUnique, userSettigns.email.isUnique);
            passwordHash = (userCompleted.passwordHash, userSettigns.passwordHash.isUnique, userSettigns.passwordHash.isUnique);
            imageSrc = (userCompleted.imageSrc, userSettigns.imageSrc.isUnique, userSettigns.imageSrc.isUnique);

            database = new Database();
        }

        public void Save()
        {
            try
            {
                CheckRequireness();
                CheckUniqueness();

                CompletedUser newUser = new CompletedUser(login.value, email.value, passwordHash.value, imageSrc.value);

                List<CompletedUser> existedUsers = database.LoadUsers();

                existedUsers.Add(newUser);

                database.SaveUsers(existedUsers);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }
        }
    }
}
