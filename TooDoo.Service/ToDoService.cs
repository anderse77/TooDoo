using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TooDoo.Entities;
using TooDoo.Data;
using System.Configuration;
using System.ServiceModel.Web;
using System.Net;

namespace TooDoo.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ToDoService : IToDoService
    {
        //ändra denna till er egen efter att i laddat ned från servern.
        private string _connectionString = "Data Source=(local);Initial Catalog = DB_ToDoList; Integrated Security = True;";
        private DAL context;

        #region WCF Service Methods

        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string name) //TODO: Anthon, name -> listName?
        {
            //Anthon: Denna check behövs inte för parametrar som bygger upp url typ "todos/{name}" Däremot kan om man skriver "todo?ListName={name}" bli null.
            //Parametrar som skickas utanför url med post eller put kan också bli null.
            //if(name == null)
            //{
            //    throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            //}

            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(name);

            if (todoListResult == null) //TODO: Anthon: byta till CheckDALError?
            {
                throw new WebFaultException<string>(context.GetErrorMessage() ,HttpStatusCode.InternalServerError);
            }

            return todoListResult;
        }

        /// <summary>
        /// Adds a todo item
        /// </summary>
        /// <param name="todo"></param>
        public void AddTodoItem(ToDo todo)
        {
            CheckInput(todo);
            context = new DAL(_connectionString);

            context.AddToDo(todo);

            if(context.GetErrorMessage() != null) //TODO: Anthon: byta till CheckDALError?
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Adds multiple todo items
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="todo"></param>
        public void AddMultipleTodoItems(string listName, List<ToDo> todo) //TODO: Anthon: variabelnamn bör visa på att det är en samling todos. todoList t.ex.
        {
            CheckInput(todo);
            context = new DAL(_connectionString);

            if (context.GetToDoListByName(listName) == null) //TODO: Antho: null returneras bara om xception händer i DAL. För att kolla felaktigt listName kolla context.GetToDoListByName(listName).Count == 0.
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            foreach (var item in todo)
            {
                try
                {
                    context.AddToDo(item);
                }
                catch (Exception ex) //TODO: Anthon: Detta exception kan aldrig inträffa, DAL kastar inte exceptions, kolla GetErrorMessage() != null istället.
                {
                    throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
                }
            }
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="id"></param>
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
            if (context.GetToDoListByName(listName) == null
                || context.GetToDoById(Convert.ToInt32(id)) == null) 
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            try
            {
                context.DeleteToDo(Convert.ToInt32(id)); //TODO: Anthon: Har gjort en metod ParseInt för detta med exception.
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest); //Anthon ändrat till BadRequest, exception händer endast då man inte får en int som id.
            }

            if (context.GetErrorMessage() != null) //TODO: Anthon: byta till CheckDALError?
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sets a todo as finished
        /// </summary>
        /// <param name="id"></param>
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
        /// Gets the number of not finished and finished todos for a specific list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public int GetNumberTodosNotFinishedByListName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);            

            return todos.Where(x => x.Finnished == false).Count();
        }

        /// <summary>
        /// Gets the number of not finished and finished todos for a specific list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public int GetNumberTodosFinishedByListName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return todos.Where(x => x.Finnished == true).Count();
        }

        /// <summary>
        /// Method for editing existing todo object
        /// </summary>
        /// <param name="todo"></param>
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
        /// <param name="listName"></param>
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
