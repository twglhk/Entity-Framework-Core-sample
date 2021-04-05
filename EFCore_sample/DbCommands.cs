using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore_sample
{
    class DbCommands
    {
        public static void InitializeDB(bool forceReset = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                // Check if DB exists
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();    // Delete DB
                db.Database.EnsureCreated();    // Create DB with new 'Data Modeling'

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        // CREATE
        public static void CreateTestData(AppDbContext db)
        {
            var player = new Player()
            {
                Name = "Humba"
            };

            // PK의 경우 별도 설정 X
            var items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreateDate = DateTime.Now,
                    Owner = player
                },
                new Item()
                {
                    TemplateId = 102,
                    CreateDate = DateTime.Now,
                    Owner = player
                },
                new Item()
                {
                    TemplateId = 103,
                    CreateDate = DateTime.Now,
                    Owner = new Player() { Name = "Ba" }
                }
            };
                                        
            db.items.AddRange(items);   // 내부에 연결된 Player 데이터도 참조해서 DB에 저장 
            db.SaveChanges();
        }

        // READ
        public static void ReadAll()
        {
            using (var db = new AppDbContext())
            {
                // AsNoTracking : ReadOnly << ignore Tracking Snapshot
                // Include : Eager Loading
                foreach(Item item in db.items.AsNoTracking().Include(i=>i.Owner))
                {
                    Console.WriteLine($"TemplateId({item.TemplateId}) Owner({item.Owner.Name}) Created({item.CreateDate})");
                }
            }
        }

        // UPDATE
        public static void UpdateDate()
        {
            Console.WriteLine("Input Player Name");
            Console.Write("> ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var items = db.items.Include(i => i.Owner)
                            .Where(i => i.Owner.Name == name);

                foreach (Item item in items)
                {
                    item.CreateDate = DateTime.Now;
                }

                db.SaveChanges();
            }

            ReadAll();
        }

        // DELETE
        public static void DeleteItem()
        {
            Console.WriteLine("Input Player Name");
            Console.Write("> ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var items = db.items.Include(i => i.Owner)
                            .Where(i => i.Owner.Name == name);

                db.items.RemoveRange(items);
                db.SaveChanges();
            }

            ReadAll();
        }
    }
}