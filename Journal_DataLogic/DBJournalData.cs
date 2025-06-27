using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Journal_Common;

namespace Journal_DataLogic
{
    public class DBJournalData : IJournalData
    {
        private static string connectionString =
            "Data Source=arianmance\\SQLEXPRESS;Initial Catalog=JournalDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public DBJournalData()
        {
            EnsureTablesExist();
        }

        private void EnsureTablesExist()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Accounts' AND xtype='U')
                    CREATE TABLE Accounts (
                        Username NVARCHAR(100) PRIMARY KEY,
                        Password NVARCHAR(50)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='JournalEntries' AND xtype='U')
                    CREATE TABLE JournalEntries (
                        Username NVARCHAR(100) NOT NULL,
                        Content NVARCHAR(MAX),
                        FOREIGN KEY (Username) REFERENCES Accounts(Username)
                    );

                    IF NOT EXISTS (SELECT * FROM Accounts WHERE Username='yanyan')
                    INSERT INTO Accounts (Username, Password) VALUES ('yanyan', '1111');

                    IF NOT EXISTS (SELECT * FROM Accounts WHERE Username='shen')
                    INSERT INTO Accounts (Username, Password) VALUES ('shen', '2222');
                ", connection);

                command.ExecuteNonQuery();
            }
        }

        public List<JournalEntry> LoadEntries()
        {
            var entries = new List<JournalEntry>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Username, Content FROM JournalEntries", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(new JournalEntry
                        {
                            Username = reader.GetString(0),
                            Content = reader.GetString(1)
                        });
                    }
                }
            }
            return entries;
        }

        public void AddEntry(JournalEntry entry)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO JournalEntries (Username, Content) VALUES (@username, @content)", connection);
                command.Parameters.AddWithValue("@username", entry.Username);
                command.Parameters.AddWithValue("@content", entry.Content);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateEntry(int index, JournalEntry entry)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    WITH Target AS (
                        SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
                        FROM JournalEntries
                        WHERE Username = @username
                    )
                    UPDATE Target SET Content = @newContent
                    WHERE RowNum = @indexPlusOne", connection);

                command.Parameters.AddWithValue("@username", entry.Username);
                command.Parameters.AddWithValue("@newContent", entry.Content);
                command.Parameters.AddWithValue("@indexPlusOne", index + 1); 

                command.ExecuteNonQuery();
            }
        }

        public void DeleteEntry(int index, string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
                    WITH Target AS (
                        SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
                        FROM JournalEntries
                        WHERE Username = @username
                    )
                    DELETE FROM Target
                    WHERE RowNum = @indexPlusOne", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@indexPlusOne", index + 1); // 1-based

                command.ExecuteNonQuery();
            }
        }

        public List<JournalEntry> SearchEntry(string keyword)
        {
            var results = new List<JournalEntry>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Username, Content FROM JournalEntries WHERE Content LIKE @keyword", connection);
                command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new JournalEntry
                        {
                            Username = reader.GetString(0),
                            Content = reader.GetString(1)
                        });
                    }
                }
            }
            return results;
        }

        public bool HasEntries()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM JournalEntries", connection);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool ValidateAccount(string username, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM Accounts WHERE Username = @username AND Password = @password", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public void SaveEntries(List<JournalEntry> entries)
        {
            
        }
    }
}