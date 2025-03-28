using System;

namespace PersonalJournal
{
    internal class Program
    {
        static List<string> journalEntries = new List<string>();
        static string[] actions = { "[1] Add Entry", "[2] View Entries", "[3] Delete Entry", "[4] Exit" };

        static void Main(string[] args)
        {
            const string correctPassword = "1127"; 
            string userPassword;

            Console.WriteLine("WELCOME TO MY JOURNAL");

            do
            {
                Console.Write("Enter Password: ");
                userPassword = Console.ReadLine();

                if (userPassword != correctPassword)
                {
                    Console.WriteLine("Incorrect password. Please try again.\n");
                }

            } while (userPassword != correctPassword);


            DisplayActions();
            int userInput = GetUserInput();

            while (userInput != 4)
            {
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
                    default:
                        Console.WriteLine("Invalid input. Please enter between 1-4 only.");
                        break;
                }

                DisplayActions();
                userInput = GetUserInput();
            }

            Console.WriteLine("Exiting... Goodbye!");
        }

        static void DisplayActions()
        {
            Console.WriteLine("\nJOURNAL MENU");
            Console.WriteLine("--------------------");

            foreach (var action in actions)
            {
                Console.WriteLine(action);
            }
        }

        static int GetUserInput()
        {
            Console.Write("[User Input]: ");
            int userInput = Convert.ToInt16(Console.ReadLine());

            return userInput;
        }

        static void AddEntry()
        {
            Console.Write("Enter date (YYYY-MM-DD): ");
            string date = Console.ReadLine();

            Console.Write("Enter your journal entry: ");
            string text = Console.ReadLine();

            journalEntries.Add($"{date} ----- {text}"); 
            Console.WriteLine("Entry added!");
        }

        static void ViewEntries()
        {
            if (journalEntries.Count == 0) 
            {
                Console.WriteLine("No entries found.");
                return;
            }

            Console.WriteLine("\nJournal Entries:");
            for (int i = 0; i < journalEntries.Count; i++) 
            {
                Console.WriteLine($"{i + 1}. {journalEntries[i]}");
            }
        }

        static void DeleteEntry()
        {
            if (journalEntries.Count == 0)
            {
                Console.WriteLine("No entries to delete.");
                return;
            }

            ViewEntries();
            Console.Write("\nEnter the entry number to delete: ");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int index) || index < 1 || index > journalEntries.Count)
            {
                Console.WriteLine("Invalid entry number.");
                return;
            }

            journalEntries.RemoveAt(index - 1);
            Console.WriteLine("Entry deleted!");
        }
    }
}