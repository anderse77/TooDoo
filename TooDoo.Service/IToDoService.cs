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

        //Behövs verkligen requestformat på GET?
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
        [WebInvoke(Method = "DELETE", UriTemplate = "todo/{id}")]
        void DeleteToDoItem(string id);

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
            UriTemplate = "todo/remainingandfinished/{name}", 
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
    }
}
