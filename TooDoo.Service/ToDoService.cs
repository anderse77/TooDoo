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
        private string _connectionString = @"Data Source = (local); Initial Catalog = DB_ToDoList; User ID = RestFullUser; Password = RestFull123";
        private DAL context;       

        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string name)
        {
            if(name == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(name);

            if (todoListResult == null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage() ,HttpStatusCode.SeeOther);
            }

            return todoListResult;
        }

        /// <summary>
        /// Adds a todo item
        /// </summary>
        /// <param name="todo"></param>
        public void AddTodoItem(ToDo todo)
        {
            if (todo == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            context.AddToDo(todo);

            if(context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }
        }

        /// <summary>
        /// Adds multiple todo items
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="todo"></param>
        public void AddMultipleTodoItems(string listName, List<ToDo> todo)
        {
            context = new DAL(_connectionString);

            if (String.IsNullOrEmpty(listName)
                || todo == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            if (context.GetToDoListByName(listName) == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            foreach (var item in todo)
            {
                try
                {
                    context.AddToDo(item);
                }
                catch (Exception ex)
                {
                    throw new WebFaultException<string>(ex.Message, HttpStatusCode.SeeOther);
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

            if (String.IsNullOrEmpty(listName)
                || String.IsNullOrEmpty(id))
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            if (context.GetToDoListByName(listName) == null
                || context.GetToDoById(Convert.ToInt32(id)) == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            try
            {
                context.DeleteToDo(Convert.ToInt32(id));
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.SeeOther);
            }

            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }
        }

        public void MarkToDoItemAsFinished(string id)
        {
            if (id == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            try
            {
                ToDo toDo = context.GetToDoById(Convert.ToInt32(id));
                toDo.Finnished = true;
                context.UpdateToDo(toDo);
            }
            catch(Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.SeeOther);
            }

            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }
        }

        /// <summary>
        /// Gets the complete list of todos
        /// </summary>
        /// <returns></returns>
        public List<ToDo> GetCompleteList()
        {
            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoList();

            if (todoListResult == null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }

            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }

            return todoListResult;
        }


        /// <summary>
        /// Gets the number of not finished and finished todos for a specific list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tuple<int, int> GetNumberTodoLeftAndFinishedinListByName(string name)
        {
            if (name == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(name);

            if (todos == null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }

            int todosLeft = todos.Where(x => x.Finnished == false).Count();
            int todosFinished = todos.Where(x => x.Finnished == true).Count();

            return new Tuple<int, int>(todosLeft, todosFinished);
        }

        public void EditToDo(ToDo todo)
        {
            if (todo == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            context.UpdateToDo(todo);

            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }
        }

        public List<ToDo> GetCompleteListOfFinishedByName(string name)
        {
            if (name == null)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }

            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(name);

            if (todoListResult == null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }

            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.SeeOther);
            }

            return todoListResult.Where(x => x.Finnished).ToList();
        }
    }
}
