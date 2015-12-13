using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TooDoo.Entities;

namespace TooDoo.SelfTest
{
    public class WcfRequestHandler
    {
        /// <summary>
        /// Returns a todo list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ToDo> GetTodoListByName(string name)
        {
            string todoList = this.sendRequest("/todo/" + name, "GET", null);
            List<ToDo> result = new JavaScriptSerializer().Deserialize<List<ToDo>>(todoList);

            return result;
        }

        /// <summary>
        /// Adds a todo list item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="deadLine"></param>
        /// <param name="estimationTime"></param>
        /// <returns></returns>
        public ToDo AddTodoItem(string name, string description, DateTime deadLine, int estimationTime)
        {
            string todo = this.sendRequest("/todo/", "POST",
                new ToDo
                {
                    CreatedDate = DateTime.Now,
                    Name = name,
                    Description = description,
                    DeadLine = deadLine,
                    EstimationTime = estimationTime
                });

            return new JavaScriptSerializer().Deserialize<ToDo>(todo);
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToDo DeleteTodoItem(string id)
        {
            string todo = this.sendRequest("/todo/" + id, "DELETE", null);

            return new JavaScriptSerializer().Deserialize<ToDo>(todo);
        }

        /// <summary>
        /// Sends request to web service
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private string sendRequest(string url, string method, object request)
        {
            ServiceHost host = new ServiceHost(typeof(Services.ToDoService));
            host.Open();

            var baseUrl = host.BaseAddresses[0].AbsoluteUri;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(baseUrl + url);
            req.Method = method;
            req.ContentType = "application/json; charset=utf-8";
            req.Accept = "application/json";

            if (request != null)
            {
                using (StreamWriter streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(request);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            using (HttpWebResponse httpResponse = (HttpWebResponse)req.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
