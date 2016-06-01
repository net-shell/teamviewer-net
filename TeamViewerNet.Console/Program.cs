using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Models.AuthCredentials authForm = new Models.AuthCredentials()
            {
                Username = "balkanski@outlook.com",
                Password = "b4lk4n$k1"
            };
            // Token in => TeamViewerNet.Config

            /*
            Console.WriteLine("Username:");
            authForm.Username = Console.ReadLine();
            Console.WriteLine("Password:");
            authForm.Password = Console.ReadLine();
            */

            //AppState state = null;
            //state = StateManager.Load();
            API api = new API();//state);
            if (api.Authenticate(authForm))
            {
                var ok = api.Ping();
                var groups = api.GetGroups(new Models.Request.GetGroupsRequest { });
                // StateManager.Save(api.State);
            }
            else
            {
                Console.WriteLine("Auth Error");
                Console.ReadKey();
            }
        }
    }
}
