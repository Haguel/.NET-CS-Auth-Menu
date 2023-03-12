using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Controllers
    {
        public User register(User user) { return new User(); }
        public User login(User user) { return new User(); }
        public User changePassword(User user, string oldPassword, string newPassword) { return new User(); }
        public void delete(User user) { }
    }
}
