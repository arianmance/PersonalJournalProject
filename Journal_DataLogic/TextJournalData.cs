using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Journal_Common;

namespace Journal_DataLogic
{
    public class TextJournalData : IJournalData
    {
        private readonly string path = "journalEntries.txt";
        private readonly List<JournalEntry> entries = new();

        private readonly Dictionary<string, string> accounts = new()
        {
            { "yanyan", "1111" },
            { "shen", "2222" }
        };

        public TextJournalData()
        {
            LoadEntries();
        }

        public List<JournalEntry> LoadEntries()
        {
            entries.Clear();
            if (!File.Exists(path)) return entries;

            foreach (var line in File.ReadAllLines(path))
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                    entries.Add(new JournalEntry { Username = parts[0], Content = parts[1] });
            }

            return entries;
        }

        private void SaveEntries()
        {
            File.WriteAllLines(path, entries.Select(e => $"{e.Username}|{e.Content}"));
        }

        public void AddEntry(JournalEntry entry)
        {
            LoadEntries();
            entries.Add(entry);
            SaveEntries();
        }

        public void UpdateEntry(int userIndex, JournalEntry entry)
        {
            LoadEntries();

            var userEntries = entries
                .Select((e, i) => new { Entry = e, Index = i })
                .Where(x => x.Entry.Username == entry.Username)
                .ToList();

            if (userIndex >= 0 && userIndex < userEntries.Count)
            {
                int actualIndex = userEntries[userIndex].Index;
                entries[actualIndex] = entry;
                SaveEntries();
            }
        }

        public void DeleteEntry(int index, string username)
        {
            LoadEntries();

            var userEntries = entries.Where(e => e.Username == username).ToList();

            if (index >= 0 && index < userEntries.Count)
            {
                var entryToRemove = userEntries[index];
                entries.Remove(entryToRemove);
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
    }
}