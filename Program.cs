using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App;

namespace exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserModel user = new UserModel(("Tsugaru", "Hammanov.gleb@gmail.com", "some hash", "htt..df"));
            user.save();
            //user.save();
        }
    }
}
