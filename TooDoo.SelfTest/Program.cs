using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;
using TooDoo.Services;

namespace TooDoo.SelfTest
{
	class Program
	{
		static void Main(string[] args)
		{
            var wcfRh = new WcfRequestHandler();
            // Returns a todo list by name
            //var result = wcfRh.GetTodoListByName("Kalle");

            // Adds a todo list item
            //wcfRh.AddTodoItem("Benke", "Röka cigarr", DateTime.Now.AddDays(2), 2);

            // Deletes a todo list item by id
            //wcfRh.DeleteTodoItem("1007");



            //IToDoService tddoWCF = new Services.ToDoService();
            //List<ToDo> list = tddoWCF.GetToDoList("Hamid");

            //Console.WriteLine(list[0].Description);

            //Console.ReadKey();

            try
            {
                ServiceHost host = new ServiceHost(typeof(Services.ToDoService));
                host.Open();
                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                host.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
