using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;
using TooDoo.Services;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                PrintMenu();
                int input = AskUserForNumericInput();
                ProcessSelection(input);
            } while (true);
        }

        public static void PrintMenu()
        {
            Console.WriteLine("TooDoo Services");
            Console.WriteLine("===============");
            Console.WriteLine("(1) Hämta att-göra-lista");
            Console.Write("Mata in en siffra beroende på vad du vill göra: ");
        }

        public static void ProcessSelection(int input)
        {          
            switch (input)
            {
                case 1:
                    PrintToDoListByUserGivenName();
                    break;
                default:
                    Console.WriteLine();
                    Console.Write("Du måste mata in en siffra som svarar mot ett alternativ på menyn!");
                    break;
            }
        }

        public static void PrintToDoListByUserGivenName()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta: ");
            string name = Console.ReadLine();
            ToDoService service = new ToDoService();
            List<ToDo> toDo = service.GetToDoListByName(name);
            if (toDo.Count == 0)
            {
                Console.WriteLine($"The todo-list with name {name} does not exist!");
            }
            toDo.ForEach(s =>
            {
                string finished = s.Finnished ? "Finished" : "Not finished";
                Console.WriteLine($"Description: {s.Description} " +
                                  $"Estimation time: {s.EstimationTime} " +
                                  $"Created: {s.CreatedDate} " +
                                  $"Deadline: {s.DeadLine} " +
                                  $"{finished}");
            });
        }

        public static int AskUserForNumericInput()
        {
            int inputAsNumber;
            bool inputWasNumber;
            do
            {
                string input = Console.ReadLine();
                inputWasNumber = int.TryParse(input, out inputAsNumber);
                if (!inputWasNumber)
                {
                    Console.WriteLine();
                    Console.Write("Du måste mata in en siffra! Försök igen: ");
                }
            } while (!inputWasNumber);
            return inputAsNumber;
        }

        public static string AskUserForInput(Predicate<string> condition, string errorMessage)
        {
            bool conditionWasMet;
            string input;
            do
            {
                input = Console.ReadLine();
                conditionWasMet = condition(input);
                if (!conditionWasMet)
                {
                    Console.WriteLine();
                    Console.Write($"{errorMessage} Försök igen: ");
                }
            } while (!conditionWasMet);
            return input;
        }
    }
}
