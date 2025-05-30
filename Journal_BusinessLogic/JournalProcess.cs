using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal_Common;
using Journal_DataLogic;

namespace Journal_BusinessLogic
{
    public class JournalProcess 
    {
        private readonly IJournalData _data;
        private string _currentUser;

        public JournalProcess()
        {
            _data = new InMemoryJournalData(); 
        }

        public bool Login(string username, string password)
        {
            bool valid = _data.ValidateUser(username, password);
            if (valid)
                _currentUser = username;
            return valid;
        }

        public bool AddEntry(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return false;
            _data.AddEntry(_currentUser, new JournalEntry(content));
            return true;
        }

        public List<string> GetEntries()
        {
            var entries = _data.GetEntries(_currentUser);
            List<string> formatted = new();
            foreach (var e in entries)
                formatted.Add($"{e.CreatedAt:yyyy-MM-dd HH:mm:ss} - {e.Content}");
            return formatted;
        }

        public bool DeleteEntry(int index)
        {
            var entries = _data.GetEntries(_currentUser);
            if (index < 1 || index > entries.Count) return false;
            _data.DeleteEntry(_currentUser, index - 1);
            return true;
        }

        public bool UpdateEntry(int index, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent)) return false;
            var entries = _data.GetEntries(_currentUser);
            if (index < 1 || index > entries.Count) return false;
            _data.UpdateEntry(_currentUser, index - 1, newContent);
            return true;
        }

        public List<string> SearchEntries(string keyword)
        {
            var matches = _data.SearchEntries(_currentUser, keyword);
            return matches.Select(e => $"{e.CreatedAt:yyyy-MM-dd HH:mm:ss} - {e.Content}").ToList();
        }
    }
}