using exam;
using System;

namespace App
{
    internal class Controllers
    {
        private Bcrypt bcrypt;
        private Validators validators;
        private Database database;

        private void CheckDataExists(BaseUser user)
        {
            UserSettings userSettings = new UserSettings();

            if (user.login == null && userSettings.login.isRequired) { throw new Exception("Login is required. Please try again."); }
            if (user.email == null && userSettings.email.isRequired) { throw new Exception("Email is required. Please try again."); }
            if (user.password == null && userSettings.passwordHash.isRequired) { throw new Exception("Password is required. Please try again."); }
            if (user.imageSrc == null && userSettings.imageSrc.isRequired) { throw new Exception("Image source is required. Please try again."); }
        }

        private void CheckDataCorrect(BaseUser user)
        {

            const int minLoginLenght = 3;
            const int maxLoginLenght = 30;

            if (user.login != null)
            {
                validators.MinLength(user.login, minLoginLenght, $"Login lenght must be at least {minLoginLenght} symbols.");
                validators.MaxLength(user.login, maxLoginLenght, $"Login lenght mustn't be bigger than {maxLoginLenght} symbols.");
            }

            if (user.email != null)
            {
                validators.IsEmail(user.email, "Email is invalid.");
            }

            if (user.password != null)
            {
                validators.IsStrongPassword(user.password, "Password isn't strong enough. It must contain digits and letter. Lenght must be at least 8 symbols.");
            }
        }

        private string makePasswordHash(string password)
        {
            string salt = bcrypt.GenSalt(10);
            return bcrypt.Hash(password, salt);
        }

        public Controllers() 
        { 
            bcrypt = new Bcrypt();
            validators = new Validators();
            database = new Database();
        }

        public CompletedUser Register(BaseUser user)
        {
            CheckDataExists(user);
            CheckDataCorrect(user);

            string passwordHash = makePasswordHash(user.password);

            CompletedUser userCompleted = new CompletedUser(user.login, user.email, passwordHash, user.email);

            UserModel userModel = new UserModel(userCompleted);

            userModel.Save();

            return userCompleted;
        }

        public CompletedUser Login(BaseUser user) 
        {
            CompletedUser existedUser = database.FindOne(user.email);

            if(existedUser == null) throw new Exception("Wrong email or password.");

            if (!bcrypt.Verify(user.password, existedUser.passwordHash)) throw new Exception("Wrong email or password.");

            return existedUser;
        }

        public CompletedUser ChangePassword(CompletedUser existedUser, string oldPassword, string newPassword)
        { 
            if (!bcrypt.Verify(oldPassword, existedUser.passwordHash)) throw new Exception("Wrong password.");

            BaseUser userWithNewPassword = new BaseUser();
            userWithNewPassword.password = newPassword;

            CheckDataCorrect(userWithNewPassword);

            string passwordHash = makePasswordHash(newPassword);

            CompletedUser updatedUser = new CompletedUser(existedUser.login, existedUser.email, passwordHash, existedUser.imageSrc);

            database.UpdateOne(existedUser, updatedUser);

            return updatedUser;
        }

        public void Delete(CompletedUser existedUser)
        {
            database.DeleteOne(existedUser);
        }
    }
}
