using System;
using System.Threading.Tasks;

namespace CRM
{
    class Program
    {
        private static bool _isWorking = true;
        private static int _currentId;
        static async Task Main(string[] args)
        {
            while (_isWorking)
            {
                switch (ClientService.ShowMainMenu())
                {
                    case "1":
                    {
                        
                    }break;
                    case "2":
                    {

                    }
                        break;
                    case "3":
                    {

                    }break;
                    case "4": _isWorking = false; break;
                }
            }
        }
    }
}