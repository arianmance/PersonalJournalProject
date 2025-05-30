using System;
using System.Collections.Generic;
using Journal_BusinessLogic; 
using Journal_Common;        
using Journal_DataLogic;     

namespace PersonalJournal
{
    internal class Program
    {
        static JournalProcess journalTask = new JournalProcess();

        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Welcome to Your Personal Journal!");
            Console.WriteLine("Keep your thoughts and memories safe.");
            Console.WriteLine("-----------------------------------------------------------");

            bool isAuthenticated = false;

            while (!isAuthenticated)
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Password: ");
                string password = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("\nUsername and password cannot be empty. Please try again.\n");
                    continue;
                }

                isAuthenticated = journalTask.Login(username, password);

                if (!isAuthenticated)
                {
                    Console.WriteLine("\nInvalid credentials. Please try again.\n");
                }
            }

            Console.WriteLine("\nLogin successful! Let's begin...\n");

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        AddEntry();
                        break;
                    case "2":
                        ViewEntries();
                        break;
                    case "3":
                        DeleteEntry();
                        break;
                    case "4":
                        UpdateEntry();
                        break;
                    case "5":
                        SearchEntries();
                        break;
                    case "6":
                        Console.WriteLine("\nThank you for journaling today. Stay reflective!");
                        return;
                    default:
                        Console.WriteLine("Not a valid option! Please enter a number between 1 to 6.");
                        break;
                }

                while (true)
                {
                    Console.WriteLine("\nWould you like to go back to the menu? (Yes/No):");
                    string backToMenu = Console.ReadLine()?.Trim().ToLower() ?? "";

                    if (backToMenu == "yes")
                    {
                        Console.Clear();
                        break;
                    }
                    else if (backToMenu == "no")
                    {
                        Console.WriteLine("\nGoodbye! Keep writing your story.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please enter 'Yes' or 'No'.");
                    }
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Please select an action:");
            Console.WriteLine("[1] Add New Entry");
            Console.WriteLine("[2] View All Entries");
            Console.WriteLine("[3] Delete an Entry");
            Console.WriteLine("[4] Update an Entry");
            Console.WriteLine("[5] Search Entries");
            Console.WriteLine("[6] Exit");
            Console.Write("Your choice: ");
        }

        static void AddEntry()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.Write("Write your journal entry here: ");
            string entry = Console.ReadLine() ?? string.Empty;

            if (journalTask.AddEntry(entry))
            {
                Console.WriteLine("Entry added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add entry. Please don't leave it empty.");
            }
        }

        static void ViewEntries()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Here are your journal entries:");

            List<string> entries = journalTask.GetEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("You haven't added any entries yet.");
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {entries[i]}");
            }
        }

        static void DeleteEntry()
        {
            List<string> entries = journalTask.GetEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("You have no entries to delete.");
                return;
            }

            ViewEntries();

            Console.Write("\nEnter the entry number to delete: ");
            string input = Console.ReadLine() ?? string.Empty;

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
                Console.WriteLine("Please enter a valid number.");
            }
        }

        static void UpdateEntry()
        {
            List<string> entries = journalTask.GetEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("You have no entries to update.");
                return;
            }

            ViewEntries();

            Console.Write("\nEnter the entry number to update: ");
            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int index))
            {
                Console.Write("Enter the new content: ");
                string newText = Console.ReadLine() ?? string.Empty;

                if (journalTask.UpdateEntry(index, newText))
                {
                    Console.WriteLine("Entry updated successfully!");
                }
                else
                {
                    Console.WriteLine("Update failed. Entry number may be invalid or text is empty.");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
            }
        }

        static void SearchEntries()
        {
            Console.Write("Enter keyword to search: ");
            string keyword = Console.ReadLine() ?? string.Empty;

            var results = journalTask.SearchEntries(keyword);

            Console.WriteLine("-----------------------------------------------------------");
            if (results.Count > 0)
            {
                Console.WriteLine("Search Results:");
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine("No matching entries found.");
            }
        }
    }
}