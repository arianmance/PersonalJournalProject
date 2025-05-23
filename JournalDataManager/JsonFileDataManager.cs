using JournalCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JournalDataManager
{
    class JsonFileDataManager : IJournalDataManager
    {
        static List<PersonalAccount> users = new List<PersonalAccount>();
        static string jsonFilePath = "users.json";

        public JsonFileDataManager()
        {
            ReadJsonDataFromFile();
        }

        private void ReadJsonDataFromFile()
        {
            if (!File.Exists(jsonFilePath))
            {
                users = new List<PersonalAccount>();
                return;
            }

            string jsonText = File.ReadAllText(jsonFilePath);

            users = JsonSerializer.Deserialize<List<PersonalAccount>>(jsonText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PersonalAccount>();
        }

        private void WriteJsonDataToFile()
        {
            string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(jsonFilePath, jsonString);
        }

        private int FindUserIndex(string username, string password)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == username && users[i].Password == password)
                {
                    return i;
                }
            }
            return -1;
        }

        public PersonalAccount GetUser(string username, string password)
        {
            return users.Find(u => u.Username == username && u.Password == password);
        }

        public void CreateUser(PersonalAccount account)
        {
            if (users.Any(u => u.Username == account.Username))
                throw new InvalidOperationException("User already exists.");

            users.Add(account);
            WriteJsonDataToFile();
        }

        public List<PersonalAccount> GetAllUsers()
        {
            return users;
        }

        public void RemoveUser(PersonalAccount account)
        {
            var index = FindUserIndex(account.Username, account.Password);

            if (index >= 0)
            {
                users.RemoveAt(index);
                WriteJsonDataToFile();
            }
        }

        public void AddEntry(PersonalAccount user, string entry)
        {
            var index = FindUserIndex(user.Username, user.Password);

            if (index >= 0)
            {
                users[index].JournalEntries.Add(entry);
                WriteJsonDataToFile();
            }
        }

        public List<string> GetEntries(PersonalAccount user)
        {
            var index = FindUserIndex(user.Username, user.Password);

            if (index >= 0)
                return users[index].JournalEntries;

            return new List<string>();
        }

        public void UpdateEntry(PersonalAccount user, int index, string newEntry)
        {
            var userIndex = FindUserIndex(user.Username, user.Password);

            if (userIndex >= 0 && index >= 0 && index < users[userIndex].JournalEntries.Count)
            {
                users[userIndex].JournalEntries[index] = newEntry;
                WriteJsonDataToFile();
            }
        }

        public void DeleteEntry(PersonalAccount user, int index)
        {
            var userIndex = FindUserIndex(user.Username, user.Password);

            if (userIndex >= 0 && index >= 0 && index < users[userIndex].JournalEntries.Count)
            {
                users[userIndex].JournalEntries.RemoveAt(index);
                WriteJsonDataToFile();
            }
        }
    }
}