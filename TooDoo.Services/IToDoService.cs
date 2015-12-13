using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;

namespace TooDoo.Services
{
	[ServiceContract]
	public interface IToDoService
	{
		[OperationContract]
		List<ToDo> GetToDoList(string name);

		[OperationContract]
		bool CreateToDoList(string name);

	}
}
