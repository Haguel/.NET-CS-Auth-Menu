using Newtonsoft.Json;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Database
    {
        private string jsonRoute = "../../data.json";

        private bool IsDB_Existed()
        {
            try { LoadUsers(); }
            catch { return false; }

            return true;
        }

        public Database() 
        {
            if (!IsDB_Existed()) SaveUsers(new List<CompletedUser>());
        }

        public List<CompletedUser> LoadUsers()
        {
            string dbJson = File.ReadAllText(jsonRoute);

            return JsonConvert.DeserializeObject<List<CompletedUser>>(dbJson);
        }

        public void SaveUsers(List<CompletedUser> users)
        {
            string dbJson = JsonConvert.SerializeObject(users);

            File.WriteAllText(jsonRoute, dbJson);
        }

        public CompletedUser FindOne(string email)
        {
            List<CompletedUser> existedUsers = LoadUsers();

            foreach (CompletedUser existedUser in existedUsers)
            {
                if (existedUser.email == email) return existedUser;
            }

            return null;
        }

        public void UpdateOne(CompletedUser existedUser, CompletedUser updatedUser)
        {
            try
            {
                if (FindOne(existedUser.email) == null)
                {
                    throw new Exception("UserModel, updateOne() error: User wasn't found.");
                }

                DeleteOne(existedUser);

                try 
                {
                    new UserModel(updatedUser).Save();
                }
                catch (Exception err) 
                {
                    new UserModel(existedUser).Save();

                    throw new Exception(err.Message);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }

        }

        public void DeleteOne(CompletedUser existedUser)
        {
            try
            {
                if (FindOne(existedUser.email) == null)
                {
                    throw new Exception("UserModel, deleteOne() error: User wasn't found.");
                }

                List<CompletedUser> existedUsers = LoadUsers();

                existedUsers.Remove(existedUser);

                SaveUsers(existedUsers);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

                Environment.Exit(0);
            }
        }
    }
}
