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
            context = new DAL(_connectionString);

            try
            {
                List<ToDo> todoListResult = context.GetToDoListByName(name);

                if(todoListResult == null)
                {
                    throw new FaultException(context.GetErrorMessage());
                }

                return todoListResult;
            }
            catch (Exception exception) //Kan med nuvarande DAL 2015-12-15 klass aldrig hända!
            {
                throw new FaultException(exception.Message + exception.StackTrace);
            }
        }

        /// <summary>
        /// Adds a todo item
        /// </summary>
        /// <param name="todo"></param>
        public void AddTodoItem(ToDo todo)
        {
            context = new DAL(_connectionString);

            try
            {
                context.AddToDo(todo);
            }
            catch (Exception exception) //Kan med nuvarande DAL 2015-12-15 klass aldrig hända!
            {
                throw new FaultException(exception.Message + exception.StackTrace);
            }
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        /// <param name="id"></param>
        public void DeleteToDoItem(string id)
        {
            context = new DAL(_connectionString);

            try
            {
                context.DeleteToDo(Convert.ToInt32(id));
            }
            catch (Exception exception) //Kan med nuvarande DAL 2015-12-15 klass aldrig hända!
            {
                throw new FaultException(exception.Message + exception.StackTrace);
            }
        }

        public void MarkToDoItemAsFinished(ToDo todo)
        {
            context = new DAL(_connectionString);

            try
            {
                ToDo toDo = context.GetToDoListById(todo.Id);
                toDo.Finnished = true;
                context.UpdateToDo(toDo);
            }
            catch (Exception exception)
            {                
                throw new FaultException(exception.Message + exception.StackTrace);
            }
        }

        public bool CreateToDo(string name)
        {
            throw new NotImplementedException();
        }
    }
}
