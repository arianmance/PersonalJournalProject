using Journal_Common;
using Journal_DataLogic;
using System.Collections.Generic;

namespace Journal_BusinessLogic
{
    public class JournalProcess
    {
        private List<string> journalEntries = new List<string>();

        public List<string> GetEntries()
        {
            return journalEntries;
        }

        public bool HasEntries()
        {
            return journalEntries.Count > 0;
        }

        public void AddEntry(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
            {
                journalEntries.Add(entry);
            }
        }

        public bool DeleteEntry(int index)
        {
            if (index >= 0 && index < journalEntries.Count)
            {
                journalEntries.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool UpdateEntry(int index, string newEntry)
        {
            if (index >= 0 && index < journalEntries.Count)
            {
                journalEntries[index] = newEntry;
                return true;
            }
            return false;
        }

        public bool SearchEntry(string keyword)
        {
            keyword = keyword.ToLower();
            foreach (var entry in journalEntries)
            {
                if (entry.ToLower().Contains(keyword))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ValidateAccount(string username, string password)
        {
            JournalData dataService = new JournalData();
            return dataService.ValidateAccount(username, password);
        }
    }
}