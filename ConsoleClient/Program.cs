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
        /// Starts the client application.
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
        /// Prints the menu to the screen.
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
            Console.WriteLine("(4) Hämta antal punkter som är avklarade i en att-göra-lista");
            Console.WriteLine("(5) Hämtar antalet punkter som inte är avklarade i en att-göra-lista");
            Console.WriteLine("(6) Ta bort en att-göra task");
            Console.WriteLine("(7) Hämta alla avklarade punkter i en given att-göra-lista");
            Console.WriteLine("(8) Skapa flera att-göra tasks");
            Console.WriteLine("(9) Hämta viktiga punkter från en lista.");
            Console.WriteLine("(10) Hämta alla punkter i en att-göra lista sorterade utifrån deadline");
            Console.WriteLine("(11) Editera en befintlig punkt");
            Console.WriteLine("(12) Se hur lång tid det tar att göra alla punker i en lista");
            Console.Write("Mata in en siffra beroende på vad du vill göra: ");
        }

        public static void ProcessSelection(int input)
        {
            switch (input)
            {
                case 1: PrintToDoListByUserGivenName(); break;
                case 2: CreateToDoListByUserInput(); break;
                case 3: SetToDoToFinished(); break;
                case 4: GetNumberOfTodosfinished(); break;
                case 5: GetNumberOfTodosLeft(); break;
                case 6: DeleteTodoItem(); break;
                case 7: GetFinishedToDoByUserInput(); break;
                case 8: AddMultipleToDoItems(); break;
                case 9: GetImportantTodos(); break;
                case 10: PrintToDoListByUserGivenNameOrderedByDeadLine(); break;
                case 11: EditTodo(); break;
                case 12: GetTotalTimeAndTimeWhenFinished(); break;
                default:
                    Console.WriteLine();
                    Console.Write("Du måste mata in en siffra som svarar mot ett alternativ på menyn!");
                    break;
            }
        }

        /// <summary>
        /// Get the the total time for all tasks in list and
        /// the time when all tasks will be finished
        /// </summary>
        private static void GetTotalTimeAndTimeWhenFinished()
        {
            Console.WriteLine("Ange listans namn: ");
            string listName = Console.ReadLine();

            if (!service.GetToDoListByName(listName)
                .Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", listName);
            }
            else
            {
                var time = service.GetTotalTimeAndTimeWhenFinished(listName);
                Console.WriteLine("Listan '{0}' kommer att ta {1} att färdigställa och kommer att vara färdig {2}",
                    listName,
                    time.TotalTime,
                    time.TimeWhenFinished);
            }
        }

        /// <summary>
        /// Ask user for input and then edit a todo.
        /// </summary>
        private static void EditTodo()
        {
            Console.WriteLine("Sriv IDt på den todo du vill editera: ");
            int id = AskUserForNumericInput();
            List<ToDo> allTodos = service.GetCompleteList();
            ToDo todo;
            try
            {
                todo = allTodos.Single(x => x.Id == id);
            }
            catch
            {
                Console.WriteLine("ToDo med det id finns ej!");
                return;
            }

            Console.Write("Skriv in ny beskrivning: ");
            string description = Console.ReadLine();
            if (description != "")
                todo.Description = description;

            Console.Write("Skriv in ett nytt datum för deadline(åååå-mm-dd): ");
            DateTime deadLine = AskUserForDateTime();
            if (deadLine > DateTime.Now)
                todo.DeadLine = deadLine;

            Console.Write("Skriv ned antalet minuter du tror det tar att göra klart punkten: ");
            int estimationTime = AskUserForNumericInput();
            if (estimationTime >= 0)
                todo.EstimationTime = estimationTime;


            service.EditToDo(id.ToString(), todo);
        }
        /// <summary>
        /// Gets all todos in a given list ordered by deadline and prints them to the screen.
        /// </summary>
        private static void PrintToDoListByUserGivenNameOrderedByDeadLine()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta sorterad utifrån deadline: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                List<ToDo> result = service.GetCompleteListOfToDosByListNameOrderedByDeadLine(name);
                PrintToDoList(result);
            }
        }

        /// <summary>
        /// Get all important todos in one list
        /// </summary>
        private static void GetImportantTodos()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta alla viktiga punkter ifrån: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                var importantTodos = service.GetImportantTodos(name);

                PrintToDoList(importantTodos);
            }

        }

        /// <summary>
        /// Get number of finished and unfinished todos from one list
        /// </summary>
        private static void GetNumberOfTodosfinished()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta antalet avklarade punkter i: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                var nbrFinished = service.GetNumberTodosFinishedByListName(name);
                Console.WriteLine($"Antal avklarade punkter: {nbrFinished}");
            }
        }

        /// <summary>
        /// fetches the finished todo-item in the todo-list with the name supplied by the user and prints it to the screen.
        /// </summary>
        private static void GetFinishedToDoByUserInput()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta alla avklarade punkter i: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                List<ToDo> finishedToDos = service.GetCompleteListOfFinishedByListName(name);
                PrintToDoList(finishedToDos);
            }
        }
        /// <summary>
        /// Gets the number of todo-items left in a list with the user-supplied name and prints the number of todo-items not
        /// finished to the screen.
        /// </summary>
        private static void GetNumberOfTodosLeft()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta antalet punkter som är kvar i: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                var nbrNotFinished = service.GetNumberTodosNotFinishedByListName(name);
                Console.WriteLine($"Antal punkter kvar: {nbrNotFinished}");
            }
        }
        /// <summary>
        /// fetches all todo-lists from the database and prints them to the screen.
        /// </summary>
        private static void PrintCompleteList()
        {
            List<ToDo> completeList = service.GetCompleteList();
            PrintToDoList(completeList);
        }
        /// <summary>
        /// Sets the todo-item with the user-supplied id as finished.
        /// </summary>
        private static void SetToDoToFinished()
        {
            Console.Write("Ange det todo id du vill markera som klar: ");
            int todoIdToSetAsFinished = AskUserForNumericInput();
            service.MarkToDoItemAsFinished(todoIdToSetAsFinished.ToString());
        }
        /// <summary>
        ///Creates a new todo-list using user-supplied data and adds it to the database.
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
        /// <summary>
        /// Prints the todo-list with the user-supplied name to the screen.
        /// </summary>
        public static void PrintToDoListByUserGivenName()
        {
            Console.Write("Skriv in det unika namnet på todo-listan du vill hämta: ");
            string name = Console.ReadLine();
            if (!service.GetToDoListByName(name).Any())
            {
                Console.WriteLine("Listan med namnet {0} finns ej", name);
            }
            else
            {
                List<ToDo> toDo = service.GetToDoListByName(name);
                if (toDo.Count == 0)
                {
                    Console.WriteLine($"The todo-list with name {name} does not exist!");
                }

                PrintToDoList(toDo);
            }
            
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
        /// Asks the user to write a line of text, validates that it can be parsed to an int and returns it to the user
        /// after it validate.
        /// </summary>
        /// <returns>The user supplied integer.</returns>
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
        /// Asks the user for a line of text, validates it according to the given predicate,
        /// and makes sure that the returned string validates according to the predicate.
        /// </summary>
        /// <param name="condition">The predicate used for validation.</param>
        /// <param name="errorMessage">The errormessage given to the user as long as it does not validat.</param>
        /// <returns>The validated string to be returned.</returns>
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
        /// Asks the user for a date until it validates as a date and then reutrns it.
        /// </summary>
        /// <returns>The supplied date.</returns>
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
        /// Prints a todo-list to the screen.
        /// </summary>
        /// <param name="toDoList">The list to be printed.</param>
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
