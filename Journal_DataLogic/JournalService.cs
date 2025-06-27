using System.Collections.Generic;
using Journal_Common;
using Journal_DataLogic;

namespace PersonalJournal
{
    public class JournalService
    {
        private readonly IJournalData dataService;

        public JournalService()
        {
            // dataService = new JsonJournalData(); // for JSON
            // dataService = new TextJournalData(); // for TextFile
            // dataService = new InMemoryJournalData(); // for InMemory
             dataService = new DBJournalData(); // for Database
        }

        public bool ValidateAccount(string username, string password)
        {
            return dataService.ValidateAccount(username, password);
        }

        public bool Login(string username, string password)
        {
            return ValidateAccount(username, password);
        }

        public void AddEntry(JournalEntry entry)
        {
            dataService.AddEntry(entry);
        }

        public List<JournalEntry> GetEntries()
        {
            return dataService.LoadEntries();
        }

        public void UpdateEntry(int index, JournalEntry entry)
        {
            dataService.UpdateEntry(index, entry);
        }

        public void DeleteEntry(int index, string username)
        {
            dataService.DeleteEntry(index, username);
        }

        public List<JournalEntry> SearchEntries(string keyword)
        {
            return dataService.SearchEntry(keyword);
        }
    }
}