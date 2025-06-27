using System;
using System.Collections.Generic;
using System.Linq;
using Journal_BusinessLogic;
using Journal_Common;

namespace PersonalJournal
{
    internal class Program
    {
        static JournalService journal = new JournalService();
        static string currentUsername = "";

        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Welcome to Your Personal Journal!");
            Console.WriteLine("Keep your thoughts and memories safe.");
            Console.WriteLine("-----------------------------------------------------------");

            while (true)
            {
                Console.Write("\nEnter Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Username and password cannot be empty. Please try again.");
                    continue;
                }

                if (journal.Login(username, password))
                {
                    currentUsername = username;
                    Console.WriteLine($"\nWelcome, {currentUsername}! Login successful!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid credentials. Please try again.");
                }
            }

            while (true)
            {
                DisplayMainMenu();
                string menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        CreateOrAddEntry();
                        break;
                    case "2":
                        RetrieveOrViewEntries();
                        break;
                    case "3":
                        UpdateEntry();
                        break;
                    case "4":
                        DeleteEntry();
                        break;
                    case "5":
                        SearchEntries();
                        break;
                    case "6":
                        Console.WriteLine("Thank you for journaling. See you next time!");
                        return;
                    default:
                        Console.WriteLine("Not a valid option! Please enter a number between 1 to 6 only.");
                        break;
                }

                Console.WriteLine("\nDo you want to go back to the menu? Please type Yes or No:");
                string backToMenu = Console.ReadLine().Trim().ToLower();

                if (backToMenu != "yes")
                {
                    Console.WriteLine("\nThank you! See you next time.");
                    break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\nPlease choose from the following options:");
            Console.WriteLine("[1] Add New Entry");
            Console.WriteLine("[2] View All Entries");
            Console.WriteLine("[3] Update an Entry");
            Console.WriteLine("[4] Delete an Entry");
            Console.WriteLine("[5] Search Entries");
            Console.WriteLine("[6] Exit");
        }

        static void CreateOrAddEntry()
        {
            var userEntries = journal.GetEntries()
                .Where(e => e.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine(userEntries.Count == 0
                ? "\nThis is your first entry! Please type your first journal note:"
                : "\nAdd a new journal entry:");

            string entry = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(entry))
            {
                journal.AddEntry(new JournalEntry { Username = currentUsername, Content = entry });
                Console.WriteLine("Entry added successfully!");
            }
            else
            {
                Console.WriteLine("No entry provided. Please try again.");
            }
        }

        static void RetrieveOrViewEntries()
        {
            var userEntries = journal.GetEntries()
                .Where(e => e.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (userEntries.Count == 0)
            {
                Console.WriteLine("\nNo entries to display. Why not create one?");
                return;
            }

            Console.WriteLine("\nYour Entries:");
            for (int i = 0; i < userEntries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userEntries[i].Content}");
            }
        }

        static void UpdateEntry()
        {
            var userEntries = journal.GetEntries()
                .Where(e => e.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (userEntries.Count == 0)
            {
                Console.WriteLine("\nNo entries to update.");
                return;
            }

            Console.WriteLine("\nYour Entries:");
            for (int i = 0; i < userEntries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userEntries[i].Content}");
            }

            Console.Write("Enter the number of the entry you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= userEntries.Count)
            {
                Console.Write("Enter the new content: ");
                string newContent = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newContent))
                {
                    journal.UpdateEntry(index - 1, new JournalEntry { Username = currentUsername, Content = newContent });
                    Console.WriteLine("Entry updated successfully!");
                }
                else
                {
                    Console.WriteLine("New content cannot be empty.");
                }
            }
            else
            {
                Console.WriteLine("Invalid entry number.");
            }
        }

        static void DeleteEntry()
        {
            var userEntries = journal.GetEntries()
                .Where(e => e.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (userEntries.Count == 0)
            {
                Console.WriteLine("\nNo entries to delete.");
                return;
            }

            Console.WriteLine("\nYour Entries:");
            for (int i = 0; i < userEntries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userEntries[i].Content}");
            }

            Console.Write("Enter the number of the entry you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= userEntries.Count)
            {
                journal.DeleteEntry(index - 1, currentUsername);
                Console.WriteLine("Entry deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid entry number.");
            }
        }

        static void SearchEntries()
        {
            var userEntries = journal.GetEntries()
                .Where(e => e.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (userEntries.Count == 0)
            {
                Console.WriteLine("\nNo entries to search.");
                return;
            }

            Console.Write("Enter keyword to search: ");
            string keyword = Console.ReadLine();

            var results = userEntries
                .Where(e => e.Content.IndexOf(keyword ?? "", StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No entries matched your search.");
            }
            else
            {
                Console.WriteLine("\nSearch results:");
                foreach (var entry in results)
                {
                    Console.WriteLine($"- {entry.Content}");
                }
            }
        }
    }
}