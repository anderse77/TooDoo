using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;

namespace TooDoo.SelfTest
{
	class Program
	{
	    static string baseUrl = "http://localhost:2121/toodoo";
        static WcfRequestHandler handler = new WcfRequestHandler();
		static void Main(string[] args)
		{
            var wcfRh = new WcfRequestHandler();
            // Returns a todo list by name
            //var result = wcfRh.GetTodoListByName("Kalle");

            // Adds a todo list item
            //wcfRh.AddTodoItem("Benke", "Röka cigarr", DateTime.Now.AddDays(2), 2);

            // Deletes a todo list item by id
            //wcfRh.DeleteTodoItem("1007");

            wcfRh.MarkToDoItemAsFinished("3");



            //IToDoService tddoWCF = new Services.ToDoService();
            //List<ToDo> list = tddoWCF.GetToDoList("Hamid");

            //Console.WriteLine(list[0].Description);

            //Console.ReadKey();

            try
            {
                ServiceHost host = new ServiceHost(typeof(Service.ToDoService));
                host.Open();
                Console.WriteLine("Hit any key to exit");
                Console.WriteLine($"Skapa en att-göra-lista   {baseUrl}/todo/");
                Console.WriteLine($"Att hämta en att-göra-lista   {baseUrl}/todo/name där name är det unika namnet på att-göra-listan");
                Console.WriteLine($"Att ta bort en att-göra lista {baseUrl}/todo/id där id är det unika id:et på en att-göra-lista");
                Console.ReadKey();
                host.Close();
                //ToDo toDo = handler.DeleteTodoItem("4");
                //Console.WriteLine(toDo.Name);
                //ToDo toDo = handler.MarkToDoItemAsFinnished("1");
                //Console.WriteLine(toDo.Name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
