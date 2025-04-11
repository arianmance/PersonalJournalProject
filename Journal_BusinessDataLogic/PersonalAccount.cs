using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_BusinessDataLogic
{
    public class PersonalAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> JournalEntries { get; set; }

        public PersonalAccount(string username, string password)
        {
            Username = username;
            Password = password;
            JournalEntries = new List<string>();
        }
    }
}