using System;
using System.Collections.Generic;
using JournalTask;

namespace PersonalJournal
{
    internal class Program
    {
        static string[] actions = { "[1] Add Entry", "[2] View Entries", "[3] Delete Entry", "[4] Update Entry", "[5] Search", "[6] Exit" };
        static JournalTask.JournalTask journalTask = new JournalTask.JournalTask();

        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO MY JOURNAL");
            Console.WriteLine("---------------------");

            bool isAuthenticated = false;

            while (!isAuthenticated)
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                isAuthenticated = journalTask.Login(username, password);

                if (!isAuthenticated)
                {
                    Console.WriteLine("Invalid username or password. Please try again.\n");
                }
            }

            Console.WriteLine("\nLogin successful!\n");

            int userInput;
            do
            {
                DisplayActions();
                userInput = GetUserInput();

                switch (userInput)
                {
                    case 1:
                        AddEntry();
                        break;
                    case 2:
                        ViewEntries();
                        break;
                    case 3:
                        DeleteEntry();
                        break;
                    case 4:
                        UpdateEntry();
                        break;
                    case 5:
                        SearchEntries();
                        break;
                    case 6:
                        Console.WriteLine("Exiting... Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        break;
                }

            } while (userInput != 6);
        }

        static void DisplayActions()
        {
            Console.WriteLine("\nJOURNAL MENU");
            Console.WriteLine("----------------");

            foreach (var action in actions)
            {
                Console.WriteLine(action);
            }
        }

        static int GetUserInput()
        {
            Console.Write("\n[User Input]: ");
            string input = Console.ReadLine();
            return int.TryParse(input, out int result) ? result : 0;
        }

        static void AddEntry()
        {
            Console.Write("Enter your journal entry: ");
            string entry = Console.ReadLine();

            if (journalTask.AddEntry(entry))
            {
                Console.WriteLine("Entry added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add entry. Text cannot be empty.");
            }
        }

        static void ViewEntries()
        {
            List<string> entries = journalTask.GetEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("No journal entries found.");
                return;
            }

            Console.WriteLine("\nYour Journal Entries:\n");
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {entries[i]}");
            }
        }

        static void DeleteEntry()
        {
            ViewEntries();
            Console.Write("\nEnter the entry number to delete: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                if (journalTask.DeleteEntry(index))
                {
                    Console.WriteLine("Entry deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid entry number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        static void UpdateEntry()
        {
            ViewEntries();
            Console.Write("\nEnter the entry number to update: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                Console.Write("Enter new journal text: ");
                string newText = Console.ReadLine();

                if (journalTask.UpdateEntry(index, newText))
                {
                    Console.WriteLine("Entry updated successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to update. Make sure the entry number and text are valid.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        static void SearchEntries()
        {
            Console.Write("Enter a keyword or date to search: ");
            string keyword = Console.ReadLine();

            var results = journalTask.SearchEntries(keyword);

            if (results.Count > 0)
            {
                Console.WriteLine("\nSearch Results:");
                foreach (var match in results)
                {
                    Console.WriteLine(match);
                }
            }
            else
            {
                Console.WriteLine("No matching entries found.");
            }
        }
    }
}
