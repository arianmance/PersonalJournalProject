﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal_Common;

namespace Journal_DataLogic
{
    public class InMemoryJournalData : IJournalData
    {
        private readonly List<JournalEntry> entries = new();
        private readonly Dictionary<string, string> accounts = new()
        {
            { "yanyan", "1111" },
            { "shen", "2222" }
        };

        public List<JournalEntry> LoadEntries()
        {
            return entries;
        }

        public void SaveEntries(List<JournalEntry> unused)
        { 
        }

        public bool HasEntries()
        {
            return entries.Any();
        }

        public void AddEntry(JournalEntry entry)
        {
            entries.Add(entry);
        }

        public void UpdateEntry(int userIndex, JournalEntry updatedEntry)
        {
            var userEntries = entries
                .Select((e, i) => new { Entry = e, Index = i })
                .Where(x => x.Entry.Username == updatedEntry.Username)
                .ToList();

            if (userIndex >= 0 && userIndex < userEntries.Count)
            {
                int actualIndex = userEntries[userIndex].Index;
                entries[actualIndex] = updatedEntry;
            }
        }

        public void DeleteEntry(int index, string username)
        {
            var userEntries = entries.Where(e => e.Username == username).ToList();

            if (index >= 0 && index < userEntries.Count)
            {
                var entryToRemove = userEntries[index];
                entries.Remove(entryToRemove);
            }
        }

        public List<JournalEntry> SearchEntry(string keyword)
        {
            return entries
                .Where(e => e.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public bool ValidateAccount(string username, string password)
        {
            return accounts.ContainsKey(username) && accounts[username] == password;
        }

        public List<JournalEntry> GetEntriesByUser(string username)
        {
            return entries
                .Where(e => e.Username == username)
                .ToList();
        }
    }
}