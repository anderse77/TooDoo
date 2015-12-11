using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooDoo.SelfTest.ToDoService;

namespace TooDoo.SelfTest
{
	class Program
	{
		static void Main(string[] args)
		{
			IToDoService tddoWCF = new ToDoServiceClient();
			List<ToDo> list = tddoWCF.GetToDoList("Hamid");

			Console.WriteLine(list[0].Description);

			Console.ReadKey();
		}
	}
}
