using JournalCommon;
using JournalDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JournalTask
{
    public class JournalTask
    {
        private PersonalDataManager dataLayer = new PersonalDataManager();
        private PersonalAccount currentUser = null!;

        public bool Login(string username, string password)
        {
            var user = dataLayer.GetUser(username, password);
            if (user != null)
            {
                currentUser = user;
                return true;
            }
            return false;
        }

        public bool AddEntry(string text)
        {
            if (currentUser != null && !string.IsNullOrWhiteSpace(text))
            {
                string date = DateTime.Now.ToString("MM-dd-yyyy");
                dataLayer.AddEntry(currentUser, $"{date} ---> {text}");
                return true;
            }
            return false;
        }

        public List<string> GetEntries()
        {
            return currentUser != null ? dataLayer.GetEntries(currentUser) : new List<string>();
        }

        public bool DeleteEntry(int index)
        {
            if (currentUser != null && index >= 1 && index <= currentUser.JournalEntries.Count)
            {
                dataLayer.DeleteEntry(currentUser, index - 1);
                return true;
            }
            return false;
        }

        public bool UpdateEntry(int index, string newText)
        {
            if (currentUser != null && index >= 1 && index <= currentUser.JournalEntries.Count && !string.IsNullOrWhiteSpace(newText))
            {
                string newDate = DateTime.Now.ToString("MM-dd-yyyy");
                dataLayer.UpdateEntry(currentUser, index - 1, $"{newDate} ---> {newText}");
                return true;
            }
            return false;
        }

        public List<string> SearchEntries(string keyword)
        {
            if (currentUser == null || string.IsNullOrWhiteSpace(keyword))
                return new List<string>();

            return currentUser.JournalEntries.FindAll(e =>
                e.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}