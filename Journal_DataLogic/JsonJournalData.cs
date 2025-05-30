using Journal_Common;
using System.Text.Json;

namespace Journal_DataLogic
{
    public class JsonJournalData : IJournalData
    {
        private readonly string filePath = "journal_entries.json";
        private Dictionary<string, List<JournalEntry>> userEntries = new();
        private List<User> users = new()
        {
            new User("sunday", "1234")
        };

        public JsonJournalData()
        {
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                userEntries = JsonSerializer.Deserialize<Dictionary<string, List<JournalEntry>>>(json)
                    ?? new Dictionary<string, List<JournalEntry>>();
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(userEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public bool ValidateUser(string username, string password)
        {
            return users.Any(u => u.Username == username && u.Password == password);
        }

        public List<JournalEntry> GetEntries(string username)
        {
            return userEntries.ContainsKey(username) ? userEntries[username] : new List<JournalEntry>();
        }

        public void AddEntry(string username, JournalEntry entry)
        {
            if (!userEntries.ContainsKey(username))
                userEntries[username] = new List<JournalEntry>();
            userEntries[username].Add(entry);
            SaveToFile();
        }

        public void DeleteEntry(string username, int index)
        {
            if (userEntries.ContainsKey(username) && index >= 0 && index < userEntries[username].Count)
            {
                userEntries[username].RemoveAt(index);
                SaveToFile();
            }
        }

        public void UpdateEntry(string username, int index, string newContent)
        {
            if (userEntries.ContainsKey(username) && index >= 0 && index < userEntries[username].Count)
            {
                userEntries[username][index].Content = newContent;
                SaveToFile();
            }
        }

        public List<JournalEntry> SearchEntries(string username, string keyword)
        {
            return GetEntries(username).Where(e => e.Content.Contains(keyword)).ToList();
        }
    }
}