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
		static void Main(string[] args)
		{
            var wcfRh = new WcfRequestHandler();
            // Returns a todo list by name
            //var result = wcfRh.GetTodoListByName("Kalle");

            // Adds a todo list item
            //wcfRh.AddTodoItem("Benke", "Röka cigarr", DateTime.Now.AddDays(2), 2);

            // Deletes a todo list item by id
            //wcfRh.DeleteTodoItem("1007");

            //wcfRh.MarkToDoItemAsFinished("3");

            wcfRh.AddTodoItem("Kalle", "Bada, Cykla, Simma");
        }
    }
}
