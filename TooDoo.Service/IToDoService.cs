using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;

namespace TooDoo.Service
{
	[ServiceContract]
	public interface IToDoService
	{
        [OperationContract]
        [WebGet(UriTemplate = "/GetAllTodo", ResponseFormat = WebMessageFormat.Json)]
        List<ToDo> GetCompleteList();

        [OperationContract]
        [WebGet(UriTemplate = "/{name}", ResponseFormat = WebMessageFormat.Json)]
		List<ToDo> GetToDoListByName(string name);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        void AddTodoItem(ToDo todo);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{id}")]
        void DeleteToDoItem(string id);

	    [OperationContract]
	    [WebInvoke( Method = "PUT", UriTemplate = "/finished/{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse)]
	    void MarkToDoItemAsFinished(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/NbrRemainingAndFinished/{name}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse)]
         Tuple<int, int> GetNumberTodoLeftAndFinishedinListByName(string name);
    }
}
