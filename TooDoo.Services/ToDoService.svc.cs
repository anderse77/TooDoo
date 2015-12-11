using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TooDoo.Entities;

namespace TooDoo.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class ToDoService : IToDoService
	{
		public bool CreateToDoList(string name)
		{
			return true;
		}

		public List<ToDo> GetToDoList(string name)
		{
			List<ToDo> list = new List<ToDo>();
			ToDo todo = new ToDo();
			todo.Id = 0;
			todo.Description = "Min todo beskrivning";
			todo.Name = "Hamid";

			list.Add(todo);

			return list;
		}
	}
}
