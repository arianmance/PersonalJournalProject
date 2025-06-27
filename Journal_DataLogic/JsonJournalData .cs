using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Journal_Common;

namespace Journal_DataLogic
{
    public class JsonJournalData : IJournalData
    {
        private readonly string path = "journalEntries.json";
        private readonly List<JournalEntry> entries = new();

        private readonly Dictionary<string, string> accounts = new()
        {
            { "yanyan", "1111" },
            { "shen", "2222" }
        };

        public JsonJournalData()
        {
            LoadEntries();
        }

        public List<JournalEntry> LoadEntries()
        {
            entries.Clear();

            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var list = JsonSerializer.Deserialize<List<JournalEntry>>(json);
                    if (list != null) entries.AddRange(list);
                }
                catch
                {
                }
            }
            return entries;
        }

        private void SaveEntries()
        {
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public void AddEntry(JournalEntry entry)
        {
            LoadEntries();
            entries.Add(entry);
            SaveEntries();
        }

        public void UpdateEntry(int userIndex, JournalEntry updatedEntry)
        {
            LoadEntries();

            var userEntries = entries
                .Select((e, i) => new { Entry = e, Index = i })
                .Where(x => x.Entry.Username == updatedEntry.Username)
                .ToList();

            if (userIndex >= 0 && userIndex < userEntries.Count)
            {
                int actualIndex = userEntries[userIndex].Index;
                entries[actualIndex] = updatedEntry;
                SaveEntries();
            }
        }

        public void DeleteEntry(int index, string username)
        {
            LoadEntries();

            var userEntries = entries
                .Select((e, i) => new { Entry = e, Index = i })
                .Where(x => x.Entry.Username == username)
                .ToList();

            if (index >= 0 && index < userEntries.Count)
            {
                int actualIndex = userEntries[index].Index;
                entries.RemoveAt(actualIndex);
                SaveEntries();
            }
        }

        public List<JournalEntry> SearchEntry(string keyword)
        {
            LoadEntries();
            return entries
                .Where(e => e.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public bool ValidateAccount(string username, string password)
        {
            return accounts.ContainsKey(username) && accounts[username] == password;
        }

        public bool HasEntries()
        {
            return entries.Any();
        }

        void IJournalData.SaveEntries(List<JournalEntry> unused)
        {
            SaveEntries();
        }

        public List<JournalEntry> GetEntriesByUser(string username)
        {
            LoadEntries();
            return entries
                .Where(e => e.Username == username)
                .ToList();
        }
    }
}