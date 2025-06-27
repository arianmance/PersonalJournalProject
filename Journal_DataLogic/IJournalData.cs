using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal_Common;

namespace Journal_DataLogic
{
    public interface IJournalData
    {
        List<JournalEntry> LoadEntries();
        void SaveEntries(List<JournalEntry> entries);
        bool HasEntries();
        void AddEntry(JournalEntry entry);
        void UpdateEntry(int index, JournalEntry entry);
        void DeleteEntry(int index, string username);
        List<JournalEntry> SearchEntry(string keyword);
        bool ValidateAccount(string username, string password);
    }
}