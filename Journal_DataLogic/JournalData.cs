using Journal_Common;
using System;
using System.Collections.Generic;

namespace Journal_DataLogic
{
    public class JournalData
    {
        List<JournalAccount> accounts = new List<JournalAccount>();

        public JournalData()
        {
            CreateDummyAccounts();
        }

        private void CreateDummyAccounts()
        {
            JournalAccount account1 = new JournalAccount("yanyan", "1111");
            JournalAccount account2 = new JournalAccount("shen", "2222");

            accounts.Add(account1);
            accounts.Add(account2);
        }

        public bool ValidateAccount(string Username, string Password)
        {
            foreach (var account in accounts)
            {
                if (account.Username == Username && account.Password == Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}