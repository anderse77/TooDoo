using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TooDoo.Entities;
using TooDoo.Data;

namespace TooDoo.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class ToDoService : IToDoService
	{
        DAL context = new DAL("Data Source=ANDERS-BÄRBAR;Initial Catalog=DB_ToDoList;Integrated Security=True;");
		public bool CreateToDoList(string name)
		{
			return true;
		}

		public List<ToDo> GetToDoList(string name)
		{
		    try
		    {
		        return context.GetToDoListByName(name);
		    }
		    catch (Exception exception)
		    {
		        throw new FaultException(exception.Message + exception.StackTrace);
		    }
		}
	}
}
