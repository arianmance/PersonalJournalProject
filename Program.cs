using System;

namespace PersonalJournal
{
    internal class Program
    {
        static List<string> journalEntries = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO MY PERSONAL JOURNAL");

            while (true)
            {
                string[] actions = { "[1] Add Entry", "[2] View Entries", "[3] Delete Entry", "[4] Exit" };

                foreach (var action in actions)
                {
                    Console.WriteLine(action);
                }
                Console.Write("Enter Action: ");

                if (int.TryParse(Console.ReadLine(), out int userAction))
                {

    
                switch (userAction)
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
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Try again!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid! Please enter a number.");
            }
        }
    }

        static void AddEntry()
        {
            Console.Write("Enter your journal entry: ");
            string entry = Console.ReadLine();
            journalEntries.Add(entry);
            Console.WriteLine("Entry added successfully!");
        }

        static void ViewEntries()
        {
            Console.WriteLine("\nYour Journal Entries:");
            if (journalEntries.Count == 0)
            {
                Console.WriteLine("No entries yet.");
            }
            else
            {
                for (int i = 0; i < journalEntries.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {journalEntries[i]}");
                }
            }
        }

        static void DeleteEntry()
        {
            ViewEntries();
            Console.Write("Enter the number of the entry to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= journalEntries.Count)
            {
                journalEntries.RemoveAt(index - 1);
                Console.WriteLine("Entry deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid!");
            }
        }
    }
}