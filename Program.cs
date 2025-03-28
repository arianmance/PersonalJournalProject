using System;
using System.Threading.Channels;
using Journal_BusinessDataLogic;

namespace PersonalJournal
{
    internal class Program
    {
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
                    case 4:
                        Console.WriteLine("Exiting... Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Incorrect input. Please enter between 1-4 only.");
                        break;
                }

                DisplayActions();
                userInput = GetUserInput();
            }
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
            string input = Console.ReadLine();

            if (IsValidNumber(input)) 
            {
                return Convert.ToInt16(input);
            }
            else
            {
                Console.WriteLine("Invalid Input.");
                return 0;
            }    
        }

        static bool IsValidNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        static void AddEntry()
        {
            Console.WriteLine("----------------");
            Console.WriteLine("ADD JOURNAL ENTRY");

            Console.Write("Enter Date (MM-DD-YYYY): ");
            string date = Console.ReadLine();


            Console.Write("Enter your journal entry: ");
            string text = Console.ReadLine();

            if (JournalFlow.AddEntry(date, text))
            {
                Console.WriteLine("Entry added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid input. Both date and text are required.");
            }
        }

        static void ViewEntries()
        {
            var entries = JournalFlow.GetJournalEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("No entries found.");
                return;
            }

            Console.WriteLine("\nJournal Entries:");
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {entries[i]}");
            }
        }

        static void DeleteEntry()
        {
            var entries = JournalFlow.GetJournalEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("No entries to delete.");
                return;
            }

            ViewEntries();

            Console.Write("\nEnter the entry number to delete: ");
            string input = Console.ReadLine();

            if (IsValidNumber(input))
            {
                int index = Convert.ToInt32(input);

                if (JournalFlow.DeleteEntry(index))
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
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}