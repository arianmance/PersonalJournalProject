using Journal_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_DataLogic
{
    public class InMemoryJournalData : IJournalData
    {
        private readonly Dictionary<string, List<JournalEntry>> _userEntries = new();
        private readonly List<User> _users = new()
        {
            new User("sunday", "1234") 
            // Add more users here or extend user management
        };

        public bool ValidateUser(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
        }

        public List<JournalEntry> GetEntries(string username)
        {
            return _userEntries.ContainsKey(username) ? _userEntries[username] : new List<JournalEntry>();
        }

        public void AddEntry(string username, JournalEntry entry)
        {
            if (!_userEntries.ContainsKey(username))
                _userEntries[username] = new List<JournalEntry>();
            _userEntries[username].Add(entry);
        }

        public void DeleteEntry(string username, int index)
        {
            if (_userEntries.ContainsKey(username) && index >= 0 && index < _userEntries[username].Count)
                _userEntries[username].RemoveAt(index);
        }

        public void UpdateEntry(string username, int index, string newContent)
        {
            if (_userEntries.ContainsKey(username) && index >= 0 && index < _userEntries[username].Count)
                _userEntries[username][index].Content = newContent;
        }

        public List<JournalEntry> SearchEntries(string username, string keyword)
        {
            return GetEntries(username).Where(e => e.Content.Contains(keyword)).ToList();
        }
    }
}