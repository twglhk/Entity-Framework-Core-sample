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
            var humba = new Player()
            {
                Name = "Humba"
            };

            var ba = new Player()
            {
                Name = "Ba"
            };

            var generalBa = new Player()
            {
                Name = "GenaralBa"
            };

            // PK의 경우 별도 설정 X
            var items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreateDate = DateTime.Now,
                    Owner = humba
                },
                new Item()
                {
                    TemplateId = 102,
                    CreateDate = DateTime.Now,
                    Owner = ba
                },
                new Item()
                {
                    TemplateId = 103,
                    CreateDate = DateTime.Now,
                    Owner = generalBa
                }
            };

            Guild guild = new Guild()
            {
                GuildName = "LIV",
                Members = new List<Player>() { humba, ba, generalBa }
            };

            db.items.AddRange(items);   // 내부에 연결된 Player 데이터도 참조해서 DB에 저장 
            db.Guilds.Add(guild);
            db.SaveChanges();
        }

        // READ
        public static void EagerLoading()
        {
            Console.WriteLine("Input Guild Name : ");
            Console.Write("> ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                // AsNoTracking : ReadOnly << ignore Tracking Snapshot
                // Include : Eager Loading
                // ThenInclude : Include(FK)->ThenInclude(FK)->table
                // Guild Query
                var guild = db.Guilds.AsNoTracking()
                            .Where(g => g.GuildName == name)
                            .Include(g => g.Members)
                            .ThenInclude(p => p.Item)
                            .First();

                foreach(Player player in guild.Members)
                {
                    Console.WriteLine($"ItemTemplatedId({player.Item.TemplateId} Owner({player.Name}))");
                }
            }
        }

        public static void ExplicitLoading()
        {
            Console.WriteLine("Input Guild Name : ");
            Console.Write("> ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var guild = db.Guilds.Where(g => g.GuildName == name).First();

                // Explicit (players query, select(SQL))
                db.Entry(guild).Collection(g=>g.Members).Load();

                foreach (Player player in guild.Members)
                {
                    // Explicit (item query, select(SQL))
                    db.Entry(player).Reference(p => p.Item).Load();
                }

                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"ItemTemplatedId({player.Item.TemplateId} Owner({player.Name}))");
                }
            }
        }

        public static void SelectLoading()
        {
            Console.WriteLine("Input Guild Name : ");
            Console.Write("> ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var info = db.Guilds
                    .Where(g => g.GuildName == name)
                    .Select(g => new
                    {
                        Name = g.GuildName,
                        MemberCount = g.Members.Count
                    })
                    .First();

                Console.WriteLine($"Guild Name({info.Name}), MemberCount({info.MemberCount})");
            }
        }
    }
}