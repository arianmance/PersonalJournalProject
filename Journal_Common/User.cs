using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_Common
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<JournalEntry> Entries { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Entries = new List<JournalEntry>();
        }
    }
}