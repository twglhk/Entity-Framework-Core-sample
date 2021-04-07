using System;

namespace EFCore_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DbCommands.InitializeDB(forceReset: false);

            // CRUD
            Console.WriteLine("Input command");
            Console.WriteLine("[0] Force Reset");
            //Console.WriteLine("[1] ReadAll");
            //Console.WriteLine("[2] ShowItems");

            // Loading Test
            Console.WriteLine("[1] Eager Loading");
            Console.WriteLine("[2] Explicit Loading");
            Console.WriteLine("[3] Select Loading");

            while (true)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "0":
                        DbCommands.InitializeDB(forceReset: true);
                        break;
                    case "1":
                        //DbCommands.ReadAll();
                        DbCommands.EagerLoading();
                        break;
                    case "2":
                        //DbCommands.ShowItems();
                        DbCommands.ExplicitLoading();
                        break;
                    case "3":
                        DbCommands.SelectLoading();
                        break;
                }
            }
        }
    }
}
