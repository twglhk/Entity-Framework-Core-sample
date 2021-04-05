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
            Console.WriteLine("[1] ReadAll");
            Console.WriteLine("[2] UpdateDate");
            Console.WriteLine("[3] DeleteItem");

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
                        DbCommands.ReadAll();
                        break;
                    case "2":
                        DbCommands.UpdateDate();
                        break;
                    case "3":
                        DbCommands.DeleteItem();
                        break;
                }
            }
        }
    }
}
