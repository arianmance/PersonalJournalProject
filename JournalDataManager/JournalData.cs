using Journal_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_DataLogic
{
    public class JournalData
    {
        private readonly IJournalData dataLayer;
        private string? currentUsername;

        public JournalData(IJournalData dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        public bool Login(string username, string password)
        {
            if (dataLayer.ValidateUser(username, password))
            {
                currentUsername = username;
                return true;
            }
            return false;
        }

        public bool AddEntry(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || currentUsername == null)
                return false;

            var entry = new JournalEntry(text);
            dataLayer.AddEntry(currentUsername, entry);
            return true;
        }

        public List<string> GetEntries()
        {
            if (currentUsername == null)
                return new List<string>();

            return dataLayer.GetEntries(currentUsername)
                            .Select(e => $"{e.CreatedAt:MM-dd-yyyy} ---> {e.Content}")
                            .ToList();
        }

        public bool DeleteEntry(int index)
        {
            if (currentUsername == null)
                return false;

            var entries = dataLayer.GetEntries(currentUsername);
            if (index >= 1 && index <= entries.Count)
            {
                dataLayer.DeleteEntry(currentUsername, index - 1);
                return true;
            }
            return false;
        }

        public bool UpdateEntry(int index, string newText)
        {
            if (currentUsername == null || string.IsNullOrWhiteSpace(newText))
                return false;

            var entries = dataLayer.GetEntries(currentUsername);
            if (index >= 1 && index <= entries.Count)
            {
                dataLayer.UpdateEntry(currentUsername, index - 1, newText);
                return true;
            }
            return false;
        }

        public List<string> SearchEntries(string keyword)
        {
            if (currentUsername == null || string.IsNullOrWhiteSpace(keyword))
                return new List<string>();

            return dataLayer.SearchEntries(currentUsername, keyword)
                            .Select(e => $"{e.CreatedAt:MM-dd-yyyy} -- {e.Content}")
                            .ToList();
        }
    }
}