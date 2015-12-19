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
        /// <summary>
        /// Gets all todos from all lists
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "todos/", ResponseFormat = WebMessageFormat.Json)]
        List<ToDo> GetCompleteList();

        /// <summary>
        /// Gets all todos by listName
        /// </summary>
        /// <param name="listName">The name of the todo-list to be fetched.</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "todos/{listName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		List<ToDo> GetToDoListByName(string listName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="todo"></param>
        [OperationContract]
        [WebInvoke(
            Method = "POST", 
            UriTemplate = "todos/", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        void AddTodoItem(ToDo todo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="todo"></param>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "todos/{listName}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        void AddMultipleTodoItems(string listName, List<ToDo> todo);


        [OperationContract]
        [WebInvoke(Method = "DELETE", 
            UriTemplate = "todos/{listName}/{id}")] //TODO: Anthon: borde ha URI todos/{Id}
        void DeleteToDoItem(string listName, string id);

        /// <summary>
        /// Sets a todo as finished
        /// </summary>
        /// <param name="id"></param>
	    [OperationContract]
	    [WebInvoke( 
            Method = "PUT", 
            UriTemplate = "todos/finished/{id}", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
	    void MarkToDoItemAsFinished(string id);

        /// <summary>
        /// Get number of not finished todos in a list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(
            UriTemplate = "todos/{listName}/numbernotfinished", 
            ResponseFormat = WebMessageFormat.Json)]
         int GetNumberTodosNotFinishedByListName(string listName);

        /// <summary>
        /// Get number of finished todos in a list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(
            UriTemplate = "todos/{listName}/numberfinished",
            ResponseFormat = WebMessageFormat.Json)]
        int GetNumberTodosFinishedByListName(string listName);

        /// <summary>
        /// Edit a already existing todo item.
        /// </summary>
        /// <param name="todo"></param>
        [OperationContract]
        [WebInvoke(
            Method = "PUT", 
            UriTemplate = "todos/{id}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        void EditToDo(string id, ToDo todo);

        /// <summary>
        /// Get all finished todos by listname
        /// </summary>
        /// <param name="listName">The todo-list name</param>
        /// <returns></returns>
	    [OperationContract]
	    [WebGet(
	        UriTemplate = "todos/{listName}/finished",
	        ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
	    List<ToDo> GetCompleteListOfFinishedByListName(string listName);
	}
}
