using JournalCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JournalDataManager
{
    public interface IJournalDataManager
    {
        PersonalAccount GetUser(string username, string password);
        void AddEntry(PersonalAccount user, string entry);
        List<string> GetEntries(PersonalAccount user);
        void DeleteEntry(PersonalAccount user, int index);
        void UpdateEntry(PersonalAccount user, int index, string newEntry);
    }
}