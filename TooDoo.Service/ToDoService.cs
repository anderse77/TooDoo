using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TooDoo.Entities;
using TooDoo.Data;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel.Web;
using System.Net;

namespace TooDoo.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ToDoService : IToDoService
    {
        //ändra denna till er egen efter att i laddat ned från servern.
        private string _connectionString = "Data Source=anders-bärbar;Initial Catalog=DB_ToDoList;Integrated Security=True;";
        private DAL context;

        #region WCF Service Methods

        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="listName">The name of the list to be returned from the database.</param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string listName) //TODO: Anthon, name -> listName?
        {
            //Anthon: Denna check behövs inte för parametrar som bygger upp url typ "todos/{name}" Däremot kan om man skriver "todo?ListName={name}" bli null.
            //Parametrar som skickas utanför url med post eller put kan också bli null.
            //if(name == null)
            //{
            //    throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            //}

            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(listName);

            CheckDALError();

            return todoListResult;
        }

        /// <summary>
        /// Adds a todo item
        /// </summary>
        /// <param name="todo">The todo-list to be added to the database</param>
        public void AddTodoItem(ToDo todo)
        {
            CheckInput(todo);
            context = new DAL(_connectionString);

            context.AddToDo(todo);

            CheckDALError();
        }

        /// <summary>
        /// Adds multiple todo items
        /// </summary>
        /// <param name="listName">The name of the todo-list to add multiple todo-items to.</param>
        /// <param name="todos">The todo-items to be added to the todo-list.</param>
        public void AddMultipleTodoItems(string listName, List<ToDo> todos) //TODO: Anthon: variabelnamn bör visa på att det är en samling todos. todoList t.ex.
        {
            CheckInput(todos);
            context = new DAL(_connectionString);

            if (context.GetToDoListByName(listName).Count == 0) //TODO: Antho: null returneras bara om xception händer i DAL. För att kolla felaktigt listName kolla context.GetToDoListByName(listName).Count == 0.
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            foreach (var item in todos)
            {
                context.AddToDo(item);
                CheckDALError();
            }
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        /// <param name="listName">The name of the list from which to delete an item.</param>
        /// <param name="id">The id of the item to be deleted.</param>
        public void DeleteToDoItem(string listName, string id)
        {
            context = new DAL(_connectionString);

            //if (String.IsNullOrEmpty(listName)
            //    || String.IsNullOrEmpty(id))
            //{
            //    throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            //}

            //TODO: Anthon går inte att kolla null, DAL returnerar bara null då exception inträffar.
            //Kolla istället att listan inte är tom och att todo objektet som returneras har Id != 0.
            if (context.GetToDoListByName(listName).Count == 0) 
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }
                context.DeleteToDo(ParseInt(id)); //TODO: Anthon: Har gjort en metod ParseInt för detta med exception.

            CheckDALError();
        }

        /// <summary>
        /// Sets a todo as finished
        /// </summary>
        /// <param name="id">The id of the todo-item to be set as finished.</param>
        public void MarkToDoItemAsFinished(string id)
        { 
            context = new DAL(_connectionString);

            //Get and cotrol that todo with id exists
            int idNbr = ParseInt(id);
            ToDo todo = context.GetToDoById(idNbr);
            CheckDALError();

            if (todo.Id != idNbr)
                throw new WebFaultException(HttpStatusCode.NotFound);

            //Set todo as finished
            todo.Finnished = true;

            //Update todo
            context.UpdateToDo(todo);
            CheckDALError();
        }

        /// <summary>
        /// Gets the complete list of todos
        /// </summary>
        /// <returns></returns>
        public List<ToDo> GetCompleteList()
        {
            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoList();
            CheckDALError();

            return todoListResult;
        }


        /// <summary>
        /// Gets the number of not finished todos for a specific list.
        /// </summary>
        /// <param name="listName">The name of the specific list.</param>
        /// <returns></returns>
        public int GetNumberTodosNotFinishedByListName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);            

            return todos.Count(x => x.Finnished == false);
        }

        /// <summary>
        /// Gets the number finished todos for a specific list.
        /// </summary>
        /// <param name="listName">The name of the specific list.</param>
        /// <returns></returns>
        public int GetNumberTodosFinishedByListName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return todos.Count(x => x.Finnished);
        }

        /// <summary>
        /// Method for editing existing todo object
        /// </summary>
        /// <param name="id">The id of the todo-item to edit.</param>
        /// <param name="todo">The todo-item to edit.</param>
        public void EditToDo(string id, ToDo todo)
        {
            CheckInput(todo);
            context = new DAL(_connectionString);

            int idNbr = ParseInt(id);
            ToDo todoDB = context.GetToDoById(idNbr);
            CheckDALError();

            if (todoDB.Id != idNbr)
                throw new WebFaultException(HttpStatusCode.NotFound);

            context.UpdateToDo(todo);
            CheckDALError();
        }

        /// <summary>
        /// Get all finished todos in a given todo-list
        /// </summary>
        /// <param name="listName">The name of the given todo-list.</param>
        /// <returns></returns>
        public List<ToDo> GetCompleteListOfFinishedByListName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(listName);
            CheckDALError();

            if (todoListResult.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return todoListResult.Where(x => x.Finnished).ToList();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks that a value is not null. Throws WebFaultException if it is.
        /// </summary>
        /// <param name="obj"></param>
        private void CheckInput(object obj)
        {
            if(obj == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Try parse a string to int. Throws WebFaultException if it cannot parse.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int ParseInt(string value)
        {
            int result;
            try
            {
                result = Int32.Parse(value);
            }
            catch(Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }

            return result;
        }

        /// <summary>
        /// Checks if any exception happened in DAL.
        /// </summary>
        /// <exception cref="WebFaultException{string}"></exception>
        private void CheckDALError()
        {
            if(context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}
