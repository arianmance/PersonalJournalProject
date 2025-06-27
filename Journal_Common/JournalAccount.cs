using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_Common
{
    public class JournalAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public JournalAccount(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public JournalAccount() { }
    }
}