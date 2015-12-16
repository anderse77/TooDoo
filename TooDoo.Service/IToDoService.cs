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
        [WebGet(UriTemplate = "todo/all", ResponseFormat = WebMessageFormat.Json)]
        List<ToDo> GetCompleteList();

        [OperationContract]
        [WebGet(UriTemplate = "todo/{name}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		List<ToDo> GetToDoListByName(string name);

        [OperationContract]
        [WebInvoke(
            Method = "POST", 
            UriTemplate = "todo/", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        void AddTodoItem(ToDo todo);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "todoLists/{listName}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        void AddMultipleTodoItems(string listName, List<ToDo> todo);

        [OperationContract]
        [WebInvoke(Method = "DELETE", 
            UriTemplate = "todoLists/{listName}/{id}")]
        void DeleteToDoItem(string listName, string id);

	    [OperationContract]
	    [WebInvoke( 
            Method = "PUT", 
            UriTemplate = "todo/finished/", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
	    void MarkToDoItemAsFinished(string id);

        [OperationContract]
        [WebGet(
            UriTemplate = "todo/numberofremainingandfinished/{name}", 
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
         Tuple<int, int> GetNumberTodoLeftAndFinishedinListByName(string name);

        [OperationContract]
        [WebInvoke(
            Method = "PUT", 
            UriTemplate = "todo/",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        void EditToDo(ToDo todo);

	    [OperationContract]
	    [WebGet(
	        UriTemplate = "todo/finished/{name}",
	        ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
	    List<ToDo> GetCompleteListOfFinishedByName(string name);
	}
}
