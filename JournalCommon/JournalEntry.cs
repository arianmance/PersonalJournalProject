using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_Common
{
    public class JournalEntry
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public JournalEntry(string content)
        {
            Content = content;
            CreatedAt = DateTime.Now;
        }
    }
}
