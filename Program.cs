using System;

namespace PersonalJournal
{
    internal class Program
    {
        static List<string> journalEntries = new List<string>();
        static void Main(string[] args)
        {


            while (true)
            {
                Console.WriteLine("\nWELCOME TO MY PERSONAL JOURNAL");
                Console.WriteLine("------------------------------");

                string[] actions = { "[1] Add Entry", "[2] View Entries", "[3] Delete Entry", "[4] Exit" };

                foreach (var action in actions)
                {
                    Console.WriteLine(action);
                }
                Console.Write("Enter Action: ");
                int userAction = Convert.ToInt16(Console.ReadLine());

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
            int count = journalEntries.Count;

            if (count == 0)
            {
                Console.WriteLine("No entries yet");
            }

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1}. {journalEntries[i]}");
            }

        }

        static void DeleteEntry()
        {
            ViewEntries();
            Console.Write("Enter the number of the entry to delete: ");
            string userInput = Console.ReadLine();
            int index = 0;
            bool validNumber = int.TryParse(userInput, out index);

            while (!validNumber || index <= 0 || index > journalEntries.Count)
            {
                Console.WriteLine("Invalid Input");
                return;
                userInput = Console.ReadLine();
                validNumber = int.TryParse(userInput, out index);
            }
            journalEntries.RemoveAt(index - 1);
            Console.WriteLine("Enter Deleted Succesfully!");

        }
    }
}