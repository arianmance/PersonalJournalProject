using JournalCommon;
using JournalDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JournalDataManager
{
    public class InMemoryDataManager : IJournalDataManager
    {
        private List<PersonalAccount> users = new List<PersonalAccount>
        {
            new PersonalAccount("eli", "1111"),
            new PersonalAccount("demi", "2222"),
            new PersonalAccount("sunday", "3333")
        };

        public PersonalAccount GetUser(string username, string password)
        {
            return users.Find(u => u.Username == username && u.Password == password);
        }

        public void AddEntry(PersonalAccount user, string entry)
        {
            user.JournalEntries.Add(entry);
        }

        public List<string> GetEntries(PersonalAccount user)
        {
            return user.JournalEntries;
        }

        public void DeleteEntry(PersonalAccount user, int index)
        {
            if (index >= 0 && index < user.JournalEntries.Count)
            {
                user.JournalEntries.RemoveAt(index);
            }
        }

        public void UpdateEntry(PersonalAccount user, int index, string newEntry)
        {
            if (index >= 0 && index < user.JournalEntries.Count)
            {
                user.JournalEntries[index] = newEntry;
            }
        }
    }
}
