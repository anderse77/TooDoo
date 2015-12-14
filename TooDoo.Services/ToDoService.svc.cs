using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TooDoo.Entities;
using TooDoo.Data;
using System.Configuration;

namespace TooDoo.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ToDoService : IToDoService
    {
        //ändra denna till er egen efter att i laddat ned från servern.
        DAL context = new DAL("");
        //DAL context = new DAL("");

        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string name)
        {
            try
            {
                return context.GetToDoListByName(name);
            }
            catch (Exception exception)
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
            try
            {
                context.AddToDo(todo);
            }
            catch (Exception exception)
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
            try
            {
                context.DeleteToDo(Convert.ToInt32(id));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message + exception.StackTrace);
            }
        }

        public void MarkToDoItemAsFinished(ToDo todo)
        {
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

        public bool CreateToDoList(string name)
        {
            throw new NotImplementedException();
        }
    }
}
