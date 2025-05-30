using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal_Common;
using System.IO;

namespace Journal_DataLogic
{
    public class TextJournalData : IJournalData
    {
        private readonly string filePath = "journal_entries.txt";
        private List<User> users = new List<User>();

        public TextJournalData()
        {
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (!File.Exists(filePath))
                return;

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 4)
                {
                    string username = parts[0];
                    string password = parts[1];
                    bool parsed = DateTime.TryParse(parts[2], out DateTime createdAt);
                    string content = parts[3];

                    if (!parsed) continue;

                    var user = users.FirstOrDefault(u => u.Username == username);
                    if (user == null)
                    {
                        user = new User(username, password);
                        users.Add(user);
                    }

                    user.Entries.Add(new JournalEntry(content) { CreatedAt = createdAt });
                }
            }
        }

        private void SaveToFile()
        {
            try
            {
                var lines = new List<string>();
                foreach (var user in users)
                {
                    foreach (var entry in user.Entries)
                    {
                        lines.Add($"{user.Username}|{user.Password}|{entry.CreatedAt:O}|{entry.Content}");
                    }
                }
                File.WriteAllLines(filePath, lines);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error saving file: " + ex.Message);
            }
        }

        public bool ValidateUser(string username, string password)
        {
            return users.Any(u => u.Username == username && u.Password == password);
        }

        public List<JournalEntry> GetEntries(string username)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            return user?.Entries ?? new List<JournalEntry>();
        }

        public void AddEntry(string username, JournalEntry entry)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.Entries.Add(entry);
                SaveToFile();
            }
        }

        public void DeleteEntry(string username, int index)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null && index >= 0 && index < user.Entries.Count)
            {
                user.Entries.RemoveAt(index);
                SaveToFile();
            }
        }

        public void UpdateEntry(string username, int index, string newContent)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null && index >= 0 && index < user.Entries.Count)
            {
                user.Entries[index].Content = newContent;
                SaveToFile();
            }
        }

        public List<JournalEntry> SearchEntries(string username, string keyword)
        {
            return GetEntries(username).Where(e => e.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}