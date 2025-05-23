using JournalCommon;
using System;
using System.Collections.Generic;
using System.IO;

namespace JournalDataManager
{
    public class TextFileDataManager : IJournalDataManager
    {
        string filePath = "users.txt";
        List<PersonalAccount> users = new List<PersonalAccount>();

        public TextFileDataManager()
        {
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (!File.Exists(filePath)) return;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                var account = new PersonalAccount(parts[0], parts[1]);

                if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                {
                    var entries = parts[2].Split(new[] { "<;>" }, StringSplitOptions.None);
                    account.JournalEntries.AddRange(entries);
                }

                users.Add(account);
            }
        }

        private void WriteDataToFile()
        {
            var lines = new string[users.Count];

            for (int i = 0; i < users.Count; i++)
            {
                var entries = string.Join("<;>", users[i].JournalEntries);
                lines[i] = $"{users[i].Username}|{users[i].Password}|{entries}";
            }

            File.WriteAllLines(filePath, lines);
        }

        public int FindUserIndex(PersonalAccount account)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == account.Username)
                    return i;
            }
            return -1;
        }

        public PersonalAccount GetUser(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public List<PersonalAccount> GetAllUsers()
        {
            return users;
        }

        public void CreateUser(PersonalAccount account)
        {
            if (users.Any(u => u.Username == account.Username))
                throw new InvalidOperationException("User already exists.");

            users.Add(account);
            WriteDataToFile();
        }

        public void RemoveUser(PersonalAccount account)
        {
            int index = FindUserIndex(account);

            if (index >= 0)
            {
                users.RemoveAt(index);
                WriteDataToFile();
            }
        }

        public void AddEntry(PersonalAccount user, string entry)
        {
            int index = FindUserIndex(user);

            if (index >= 0)
            {
                users[index].JournalEntries.Add(entry);
                WriteDataToFile();
            }
        }

        public List<string> GetEntries(PersonalAccount user)
        {
            int index = FindUserIndex(user);

            if (index >= 0)
                return users[index].JournalEntries;

            return new List<string>();
        }

        public void UpdateEntry(PersonalAccount user, int entryIndex, string newEntry)
        {
            int index = FindUserIndex(user);

            if (index >= 0 && entryIndex >= 0 && entryIndex < users[index].JournalEntries.Count)
            {
                users[index].JournalEntries[entryIndex] = newEntry;
                WriteDataToFile();
            }
        }

        public void DeleteEntry(PersonalAccount user, int entryIndex)
        {
            int index = FindUserIndex(user);

            if (index >= 0 && entryIndex >= 0 && entryIndex < users[index].JournalEntries.Count)
            {
                users[index].JournalEntries.RemoveAt(entryIndex);
                WriteDataToFile();
            }
        }
    }
}