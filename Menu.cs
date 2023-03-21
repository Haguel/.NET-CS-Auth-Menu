using exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Menu
    {
        private CompletedUser currentUser = null;
        private Controllers controllers = new Controllers();

        List<ConsoleKey> availableKeys = new List<ConsoleKey>() { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, };

        private bool IsValidKey(ConsoleKey key)
        {
            foreach (ConsoleKey validKey in availableKeys)
            {
                if (key == validKey) return true;
            }

            return false;
        }

        private static string ReadLineOrEsc()
        {
            string currentString = "";

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape) 
                {
                    Console.WriteLine();

                    throw new Exception();
                }

                if (keyInfo.Key == ConsoleKey.Enter) 
                {
                    Console.WriteLine();

                    return currentString;
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentString.Length > 0)
                    {
                        currentString = currentString.Remove(currentString.Length - 1);

                        Console.Write("\b \b");
                    }
                }

                else
                {
                    currentString += keyInfo.KeyChar;

                    Console.Write(keyInfo.KeyChar);
                }
            } while (true);
        }

        private string ValidateInput(string value)
        {
            return value.Trim() == "" ? null : value;
        }

        private void breakWhenKeyPressed()
        {
            while (true)
            {
                if (Console.KeyAvailable) { break; }
            }
        }

        private void OutputMenuHint(string key = "Escape")
        {
            WriteLineColorText($"Press {key} in order to get into the main menu.\n", ConsoleColor.Red);
        }

        private void OutputFinishScreen(string text)
        {
            Console.Clear();

            OutputMenuHint("any button");
            WriteLineColorText(text, ConsoleColor.Yellow);

            breakWhenKeyPressed();

            Output();
        }

        private void OutputCurrentUser()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            if(currentUser == null) Console.WriteLine($"Please log in or register.\n");
            else Console.WriteLine($"Account: {currentUser.login}.\n");

            Console.ResetColor();
        }

        private void WriteLineColorText(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void WriteColorText(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        private void Register()
        {
            BaseUser user = new BaseUser();

            OutputMenuHint();
            WriteLineColorText("Fill the form.", ConsoleColor.Cyan);

            try
            {
                WriteColorText("Login*: ", ConsoleColor.Cyan);
                user.login = ValidateInput(ReadLineOrEsc());

                WriteColorText("Email*: ", ConsoleColor.Cyan);
                user.email = ValidateInput(ReadLineOrEsc());

                WriteColorText("Password*: ", ConsoleColor.Cyan);
                user.password = ValidateInput(ReadLineOrEsc());

                WriteColorText("Image source*: ", ConsoleColor.Cyan);
                user.imageSrc = ValidateInput(ReadLineOrEsc());
            }
            catch
            {
                Output();
            }
            
            currentUser = controllers.Register(user);

            Console.Clear();

            OutputMenuHint("any button");
            WriteLineColorText("Account has been successfuly registered!", ConsoleColor.Yellow);

            breakWhenKeyPressed();

            Output();
        }

        private void Login()
        {
            BaseUser user = new BaseUser();

            OutputMenuHint();
            WriteLineColorText("Fill the form.", ConsoleColor.Cyan);

            try
            {
                WriteColorText("Email*: ", ConsoleColor.Cyan);
                user.email = ValidateInput(ReadLineOrEsc());

                WriteColorText("Password*: ", ConsoleColor.Cyan);
                user.password = ValidateInput(ReadLineOrEsc());
            }
            catch
            {
                Output();
            }

            try
            {
                currentUser = controllers.Login(user);
            }
            catch (Exception exception)
            {
                OutputFinishScreen(exception.Message);
            }

            OutputFinishScreen("You have been successfully loginned!");
        }

        private void ChangePassword() 
        {
            if (currentUser == null)
            {
                OutputFinishScreen("Please log in or register first");

                return;
            }

            OutputMenuHint();

            string oldPassword = "";
            string newPassword = "";

            try
            {
                WriteColorText("Current password*: ", ConsoleColor.Cyan);
                oldPassword = ValidateInput(Console.ReadLine());

                WriteColorText("New password*: ", ConsoleColor.Cyan);
                newPassword = ValidateInput(Console.ReadLine());
            }
            catch
            {
                Output();
            }

            currentUser = controllers.ChangePassword(currentUser, oldPassword, newPassword);

            OutputFinishScreen("You have been successfully changed the password!");
        }

        private void Delete()
        {
            if (currentUser == null)
            {
                OutputFinishScreen("Please log in or register first");

                return;
            }

            controllers.Delete(currentUser);

            currentUser = null;

            OutputFinishScreen("You have successfully deleted yout account!");
        }

        public void Output()
        {
            Console.Clear();
            OutputCurrentUser();

            WriteLineColorText("Menu", ConsoleColor.Cyan);
            WriteColorText("1. ", ConsoleColor.Cyan); WriteLineColorText("Create an account.");
            WriteColorText("2. ", ConsoleColor.Cyan);
            if (currentUser == null) WriteLineColorText("Log in."); else WriteLineColorText("Switch account.");
            WriteColorText("3. ", ConsoleColor.Cyan); WriteLineColorText("Change the password");
            WriteColorText("4. ", ConsoleColor.Cyan); WriteLineColorText("Delete current account");

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
            } while (!IsValidKey(key));

            Console.Clear();

            if (key == ConsoleKey.D1) Register();
            if (key == ConsoleKey.D2) Login();
            if (key == ConsoleKey.D3) ChangePassword();
            if (key == ConsoleKey.D4) Delete();
        }
    }
}
