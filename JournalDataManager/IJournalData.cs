using Journal_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_DataLogic
{
    public interface IJournalData
    {
        bool ValidateUser(string username, string password);
        List<JournalEntry> GetEntries(string username);
        void AddEntry(string username, JournalEntry entry);
        void DeleteEntry(string username, int index);
        void UpdateEntry(string username, int index, string newContent);
        List<JournalEntry> SearchEntries(string username, string keyword);
    }
}