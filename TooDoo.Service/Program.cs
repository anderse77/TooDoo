using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TooDoo.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uri uri = new Uri("http://localhost:2121/todo"); //Info finns i app.config
            WebServiceHost host = new WebServiceHost(typeof(ToDoService));
        //  host.AddServiceEndpoint(typeof(IToDoService)); //Info finns i app.config
            host.Open();

            Console.WriteLine($"WCF Service ToDoService running at: {host.BaseAddresses[0]}");
            Console.WriteLine("Shut down service by pressing <Escape>!");

            while (Console.ReadKey().Key != ConsoleKey.Escape) ;

            host.Close();
        }
    }
}
