using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore_sample
{
    class Program
    {
        // UDF method
        [DbFunction()]  // Data Annotation Attribute
        public static double? GetAverageReviewScore(int itemId)
        {
            throw new NotImplementedException("Do Not Use. It's for UDF");
        }

        static void Main(string[] args)
        {
            DbCommands.InitializeDB(forceReset: false);

            // CRUD
            Console.WriteLine("Input command");
            Console.WriteLine("[0] Force Reset");
            //Console.WriteLine("[1] ReadAll");
            Console.WriteLine("[2] ShowItems");

            // Loading Test
            //Console.WriteLine("[1] Eager Loading");
            //Console.WriteLine("[2] Explicit Loading");
            //Console.WriteLine("[3] Select Loading");

            // Update Test
            //Console.WriteLine("[1] Update Guild Data");
            //Console.WriteLine("[1] Update (Reload)");
            //Console.WriteLine("[2] Update (Full");
            //Console.WriteLine("[1] Dependency test");
            //Console.WriteLine("[1] Update 1vs1");
            //Console.WriteLine("[3] TestUpdateAttach");
            Console.WriteLine("[3] DirectSQLCallTest");

            // Delete Test
            //Console.WriteLine("[1] Delete");

            //Console.WriteLine("[3] CalcAvg");

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
                        //DbCommands.EagerLoading();
                        //DbCommands.UpdateByReload();
                        //DbCommands.Test();
                        //DbCommands.Update_1v1();
                        //DbCommands.Update_1vM();
                        DbCommands.DeleteTest();
                        break;
                    case "2":
                        DbCommands.ShowItems();
                        //DbCommands.ExplicitLoading();
                        //DbCommands.UpdateByFull();
                        break;
                    case "3":
                        //DbCommands.SelectLoading();
                        //DbCommands.CalcAverage();
                        //DbCommands.TestUpdateAttach();
                        DbCommands.DirectSQLCallTest();
                        break;
                }
            }
        }
    }
}
