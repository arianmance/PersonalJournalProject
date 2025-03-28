using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_BusinessDataLogic
{
    public class JournalFlow
    {
        public static List<string> journalEntries = new List<string>();

        public static List<string> GetJournalEntries()
        {
            return journalEntries;
        }

        public static bool AddEntry(string date, string text)
        {
            if (!string.IsNullOrWhiteSpace(date) && !string.IsNullOrWhiteSpace(text))
            {
                journalEntries.Add($"{date} ---> {text}");
                return true;
            }
            return false;
        }

        public static List<string> GetEntries()
        {
            return new List<string>(journalEntries);
        }

        public static bool DeleteEntry(int index)
        {
            if (index >= 1 && index <= journalEntries.Count)
            {
                journalEntries.RemoveAt(index - 1);
                return true;
            }
            return false;
        }

        public static int GetEntryCount()
        {
            return journalEntries.Count;
        }
    }
}