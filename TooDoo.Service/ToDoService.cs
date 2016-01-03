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
        private const string _connectionString = "Data Source=(local);Initial Catalog=DB_ToDoList;Integrated Security=True;";
        private DAL context;

        #region WCF Service Methods

        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="listName">The name of the list to be returned from the database.</param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(listName);
            CheckDALError();

            //DAL search listnames with wildcard this sorts out only exactly matching todos, and handle important marking.
            return GetExactMatchingTodos(todoListResult, listName);
        }

        /// <summary>
        /// Returns how long it will take to complete all tasks in list
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public string GetEstimate(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            todos = GetExactMatchingTodos(todos, listName);

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            var totalTime = todos
                .Where(x => !x.Finnished)
                .Select(x => x.EstimationTime).Sum();

            var timeSpan = TimeSpan.FromMinutes(totalTime);

            return string.Format("{0} dagar, {1} timmar, {2} minuter",
                timeSpan.Days,
                timeSpan.Hours,
                timeSpan.Minutes);
        }

        /// <summary>
        /// Returns the time when all tasks in list will be done
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public string GetTimeWhenDone(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todos = context.GetToDoListByName(listName);
            CheckDALError();

            todos = GetExactMatchingTodos(todos, listName);

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            var totalTime = todos
                .Where(x => !x.Finnished)
                .Select(x => x.EstimationTime).Sum();

            var timeSpan = TimeSpan.FromMinutes(totalTime);

            // If tasks take more than a day show date and time
            if (timeSpan.Days > 0)
            {
                return string.Format(
                    DateTime.Now
                    .AddDays(timeSpan.Days)
                    .AddHours(timeSpan.Hours)
                    .AddMinutes(timeSpan.Minutes).ToString("yyyy-MM-dd HH:mm"));
            }
            //...otherwise only show the time
            else
            {
                return string.Format(
                    DateTime.Now
                    .AddHours(timeSpan.Hours)
                    .AddMinutes(timeSpan.Minutes).ToString("HH:mm"));
            }
        }

        /// <summary>
        /// Get all important todos in todolist with name listName
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public List<ToDo> GetImportantTodos(string listName)
        {
            context = new DAL(_connectionString);

            List<ToDo> todoListResult = context.GetToDoListByName(listName);
            CheckDALError();

            return GetExactMatchingTodos(todoListResult, listName).Where(todo => NameIsImportant(todo.Name)).ToList();
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
        /// <param name="items">The items to be added to the todo-list</param>
        public void AddMultipleTodoItems(string listName, string items)
        {
            context = new DAL(_connectionString);

            var todos = new List<ToDo>();

            foreach (var item in items.Split(','))
            {
                todos.Add(
                    new ToDo
                    {
                        CreatedDate = DateTime.Now,
                        Name = listName,
                        Description = item.Trim(),
                        DeadLine = DateTime.Now.AddDays(1)
                    });
            }

            List<ToDo> todoList = context.GetToDoListByName(listName);
            todoList = GetExactMatchingTodos(todos, listName);

            if (todoList.Count == 0)
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

            if (context.GetToDoListByName(listName).Count == 0)
            {
                throw new WebFaultException<string>("Wrong method syntax", HttpStatusCode.NotFound);
            }
            context.DeleteToDo(ParseInt(id));

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

            todos = GetExactMatchingTodos(todos, listName);

            if (todos.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return todos.Count(x => !x.Finnished);
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

            todos = GetExactMatchingTodos(todos, listName);

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

            todoListResult = GetExactMatchingTodos(todoListResult, listName);

            if (todoListResult.Count == 0)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return todoListResult.Where(x => x.Finnished).ToList();
        }

        /// <summary>
        /// Gets all todos in a given list ordered by deadline.
        /// </summary>
        /// <param name="listName">The name of the given list.</param>
        /// <returns></returns>
        public List<ToDo> GetCompleteListOfToDosByListNameOrderedByDeadLine(string listName)
        {
            List<ToDo> todoListResult = GetToDoListByName(listName);

            return todoListResult.Where(t => !t.Finnished).OrderBy(t => t.DeadLine).ToList();

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks that a value is not null. Throws WebFaultException if it is.
        /// </summary>
        /// <param name="obj"></param>
        private void CheckInput(object obj)
        {
            if (obj == null)
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
            catch (Exception ex)
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
            if (context.GetErrorMessage() != null)
            {
                throw new WebFaultException<string>(context.GetErrorMessage(), HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sorts out todos that fully match the listname. Not case sensitive. "ListName" and "ListName!" will be considered equal.
        /// </summary>
        /// <param name=""></param>
        /// <param name="listName"></param>
        private List<ToDo> GetExactMatchingTodos(List<ToDo> todoList, string listNameToMatchWith)
        {
            return todoList.Where(todo => NamesAreEqual(todo.Name, listNameToMatchWith)).ToList();
        }

        /// <summary>
        /// Check if the todolistName is marked with important
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool NameIsImportant(string name)
        {
            return name.Trim()[name.Length - 1] == '!';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        private bool NamesAreEqual(string name1, string name2)
        {
            return GetNameWithoutImportantMarker(name1.ToLower()) == GetNameWithoutImportantMarker(name2.ToLower());
        }

        /// <summary>
        /// Returns a string without '!' in the end
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetNameWithoutImportantMarker(string name)
        {
            if (NameIsImportant(name))
            {
                name = name.Remove(name.Length - 1).Trim();
            }
            return name;
        }

        #endregion
    }
}
