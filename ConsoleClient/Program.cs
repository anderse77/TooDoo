using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;
using TooDoo.Service;

namespace ConsoleClient
{
    class Program
    {
        static IToDoService service;
        /// <summary>
        /// Startar konsolklienten och själva applikationen så servicen kan testas.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (ChannelFactory<IToDoService> channelFactory = new ChannelFactory<IToDoService>(new WebHttpBinding(), "http://localhost:2121"))
            {
                channelFactory.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                service = channelFactory.CreateChannel();

                Console.WindowWidth = 110;
                Console.WindowHeight = 26;

                do
                {
                    PrintMenu();
                    int input = AskUserForNumericInput();
                    ProcessSelection(input);
                    AskForAnyKeyToContinue();
                } while (true);
            }
        }
        /// <summary>
        /// Skriver ut menyn på skärmen.
        /// </summary>
        public static void PrintMenu()
        {
            Console.Clear();
            PrintCompleteList();
            Console.WriteLine();
            Console.WriteLine("TooDoo Services");
            Console.WriteLine("===============");
            Console.WriteLine("(1) Hämta att-göra-lista");
            Console.WriteLine("(2) Skapa en att-göra-task");
            Console.WriteLine("(3) Sätt en att göra task till färdig");
            Console.WriteLine("(4) Hämta antal punkter som är kvar och avklarade i en todo lista");
            Console.WriteLine("(5) Ta bort en att-göra task");
            Console.WriteLine("(6) Hämta alla avklarade punkter i en given att-göra-lista");
            Console.WriteLine("(7) Skapa flera att-göra tasks");
            Console.Write("Mata in en siffra beroende på vad du vill göra: ");
        }

        public static void ProcessSelection(int input)
        {
            switch (input)
            {
                case 1: PrintToDoListByUserGivenName(); break;
                case 2: CreateToDoListByUserInput(); break;
                case 3: SetToDoToFinished(); break;
                case 4: GetLeftAndFinishedToDo(); break;
                case 5: DeleteTodoItem(); break;
                case 6: GetFinishedToDoByUserInput(); break;
                case 7: AddMultipleToDoItems(); break;
                default:
                    Console.WriteLine();
                    Console.Write("Du måste mata in en siffra som svarar mot ett alternativ på menyn!");
                    break;
            }
        }
        /// <summary>
        /// Hämtar en given att-göra-lista med det namn som användaren anger, hämtar alla färdiga att-göra-punkter i den listan
        /// och skriver ut den på skärmen.
        /// </summary>
        private static void GetFinishedToDoByUserInput()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta alla avklarade punkter i: ");
            string name = Console.ReadLine();
            List<ToDo> finishedToDos = service.GetCompleteListOfFinishedByListName(name);
            PrintToDoList(finishedToDos);
        }

        private static void GetLeftAndFinishedToDo()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta antalet punkter kvar och antalet avklarade: ");
            string name = Console.ReadLine();
            var nbrNotFinished = service.GetNumberTodosNotFinishedByListName(name);
            Console.WriteLine($"Antal punkter kvar: {nbrNotFinished}");
        }
        /// <summary>
        /// Hämtar samtliga att-göra-listor från databasen och skriver ut dem på skärmen.
        /// </summary>
        private static void PrintCompleteList()
        {
            List<ToDo> completeList = service.GetCompleteList();
            PrintToDoList(completeList);
        }

        private static void SetToDoToFinished()
        {
            Console.Write("Ange det todo id du vill markera som klar: ");
            int todoIdToSetAsFinished = AskUserForNumericInput();

            service.MarkToDoItemAsFinished(todoIdToSetAsFinished.ToString());
        }
        /// <summary>
        /// Hämtar in data från användaren som representerar en instans av klassen ToDo. Skickar in
        /// den i databasen via servicen.
        /// </summary>
        private static void CreateToDoListByUserInput()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill skapa: ");
            string name = AskUserForInput(n => n.Length >= 6, "The name must be at least 6 characters!");
            Console.Write("Skriv in beskrivningen av den första punkten i listan: ");
            string description = Console.ReadLine();
            Console.Write("Skriv in ett datum för deadline(åååå-mm-dd): ");
            DateTime deadLine = AskUserForDateTime();
            Console.Write("Skriv ned antalet minuter du tror det tar att göra klart punkten: ");
            int estimationTime = AskUserForNumericInput();

            ToDo toDoToCreate = new ToDo
            {
                Name = name,
                CreatedDate = DateTime.Now,
                DeadLine = deadLine,
                Description = description,
                EstimationTime = estimationTime,
                Finnished = false
            };
            service.AddTodoItem(toDoToCreate);
        }

        /// <summary>
        /// Adds multiple todo items
        /// </summary>
        private static void AddMultipleToDoItems()
        {
            Console.WriteLine("Ange listan du önskar lägga till flera punkter till: ");
            string listName = Console.ReadLine();

            if (!service.GetToDoListByName(listName)
                .Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", listName);
            }
            else
            {
                Console.WriteLine("Ange beskrivning på de punkter du önskar addera till listan, separera dem med komma (,): ");
                string items = Console.ReadLine();

                List<ToDo> todoList = new List<ToDo>();

                foreach (var item in items.Split(','))
                {
                    ToDo todo = new ToDo()
                    {
                        CreatedDate = DateTime.Now,
                        Name = listName,
                        Description = item.Trim(),
                        DeadLine = DateTime.Now.AddDays(1)
                    };

                    todoList.Add(todo);
                }

                service.AddMultipleTodoItems(listName, todoList);
            }    
        }

        public static void PrintToDoListByUserGivenName()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta: ");
            string name = Console.ReadLine();
            List<ToDo> toDo = service.GetToDoListByName(name);
            if (toDo.Count == 0)
            {
                Console.WriteLine($"The todo-list with name {name} does not exist!");
            }
            toDo.ForEach(s =>
            {
                string finished = s.Finnished ? "Finished" : "Not finished";
                Console.WriteLine($"Id: {s.Id} " +
                                  $"Description: {s.Description} " +
                                  $"Estimation time: {s.EstimationTime} " +
                                  $"Created: {s.CreatedDate} " +
                                  $"Deadline: {s.DeadLine} " +
                                  $"{finished}");
            });
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        private static void DeleteTodoItem()
        {
            Console.WriteLine("Ange namnet på todolistan där du vill ta bort en punkt: ");
            string listName = Console.ReadLine();

            if (!service.GetToDoListByName(listName)
                .Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", listName);
            }
            else
            {
                Console.WriteLine("Ange det todo id du vill ta bort: ");
                int id = AskUserForNumericInput();

                service.DeleteToDoItem(listName, id.ToString());
            }
        }
        /// <summary>
        /// Ber användaren skriva in en textrad och validerar att den består av enbart siffror.
        /// När användaren skrivit in en rad bestående av enbart siffror returneras den raden.
        /// </summary>
        /// <returns>Talet användaren skrev in.</returns>
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
        /// <summary>
        /// Frågar användaren om en textrad, validerar det enligt ett givet villkor och returnerar textraden s snart
        /// abvändaren matar in något som stämmer med villkoret.
        /// </summary>
        /// <param name="condition">Villkoret som textraden ska valideras enligt.</param>
        /// <param name="errorMessage">felmeddelandet som ges till användaren så länge textraden inte validerar.</param>
        /// <returns>Textraden som skrivits in av användaren och som validerar enligt villkoret.</returns>
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

        /// <summary>
        /// Frågar en användare efter att ange ett datum och validerar att det verkligen är ett datum som skrivs in
        /// returnerar datumet snart det validerar.
        /// </summary>
        /// <returns>Datumet användaren skriver in.</returns>
        public static DateTime AskUserForDateTime()
        {
            DateTime inputAsDateTime;
            bool inputWasDateTime;
            do
            {
                string input = Console.ReadLine();
                inputWasDateTime = DateTime.TryParse(input, out inputAsDateTime);
                if (!inputWasDateTime)
                {
                    Console.WriteLine();
                    Console.Write("Du måste mata in ett giltigt datum! Försök igen: ");
                }
            } while (!inputWasDateTime);
            return inputAsDateTime;
        }

        public static void AskForAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press Any key to continue!");
            Console.ReadKey();
        }

        /// <summary>
        /// Skriver ut en att-göra-lista på skärmen.
        /// </summary>
        /// <param name="toDoList">listan som ska skrivas ut.</param>
        static void PrintToDoList(List<ToDo> toDoList)
        {
            toDoList.ForEach(x =>
            {

                Console.WriteLine($"{x.Id,-8}{x.Name,-10}{x.Description,-30}{x.CreatedDate.ToShortDateString(),-12}" +
                                  $"{x.DeadLine.ToShortDateString(),-12}{x.EstimationTime,-5}{x.Finnished}");


            });
        }
    }
}
