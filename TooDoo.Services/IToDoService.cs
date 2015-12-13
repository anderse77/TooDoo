using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Services;
using TooDoo.Entities;

namespace TooDoo.Services
{
	[ServiceContract]
	public interface IToDoService
	{
		[OperationContract]
        [WebGet(UriTemplate = "todo/{name}", ResponseFormat = WebMessageFormat.Json)]
		List<ToDo> GetToDoListByName(string name);

		[OperationContract]
		bool CreateToDoList(string name);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedResponse, Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "todo/")]
        void AddTodoItem(ToDo todo);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedResponse, Method = "DELETE", ResponseFormat = WebMessageFormat.Json, UriTemplate = "todo/{id}")]
        void DeleteTodoItem(string id);
    }
}
