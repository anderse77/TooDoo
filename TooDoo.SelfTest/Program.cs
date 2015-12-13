using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Entities;
using TooDoo.Services;

namespace TooDoo.SelfTest
{
	class Program
	{
		static void Main(string[] args)
		{
            //IToDoService tddoWCF = new Services.ToDoService();
            //List<ToDo> list = tddoWCF.GetToDoList("Hamid");

            //Console.WriteLine(list[0].Description);

            //Console.ReadKey();
            try
            {
                ServiceHost host = new ServiceHost(typeof(Services.ToDoService));
                host.Open();
                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                host.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
