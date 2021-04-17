using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

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

            // Test Shadow Property Value Write
            db.Entry(items[0]).Property("RecoveredDate").CurrentValue = DateTime.Now;

            // Test Backing Field
            items[0].SetOption(new ItemOption() { dex = 1, hp = 2, str = 3 });

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
                            .ThenInclude(p => p.OwnedItem)
                            .First();

                foreach(Player player in guild.Members)
                {
                    Console.WriteLine($"ItemTemplatedId({player.OwnedItem.TemplateId} Owner({player.Name}))");
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
                    db.Entry(player).Reference(p => p.OwnedItem).Load();
                }

                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"ItemTemplatedId({player.OwnedItem.TemplateId} Owner({player.Name}))");
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
                    .MapGuildToDto()
                    .First();

                Console.WriteLine($"Guild Name({info.Name}), MemberCount({info.MemberCount})");
            }
        }

        // UPDATE
        // 1) Get Tracked Entity
        // 2) Set Propery on Entity class
        // 3) Call SaveChanges()

        /* SQL
        1) 
        SELECT TOP(2) GuildId, GuildName
        FROM [Guilds]
        WHERE GuildName = N'LIV';
          
        2) ~ 3)
        SET NOCOUNT ON;
        UPDATE [Guilds]
        SET GuildName = @p0
        WHERE GuildId = @p1;
        SELECT @@ROWCOUNT;
         */
        //public static void UpdateTest()
        //{
        //    using (AppDbContext db = new AppDbContext())
        //    {
        //        var guild = db.Guilds.Single(g => g.GuildName == "LIV");

        //        guild.GuildName = "TOT";

        //        db.SaveChanges();
        //    }
        //}

        public static void ShowGuilds()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach(var guild in db.Guilds.MapGuildToDto())
                {
                    Console.WriteLine($"GuildId({guild.GuildId}) GuildName({guild.Name}) MemberCount({guild.MemberCount})");
                }
            }
        }

        public static void UpdateByReload()
        {
            ShowGuilds();
            Console.WriteLine("Input GuildId");
            Console.Write(" > ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Input GuildName");
            Console.WriteLine(" > ");
            string name = Console.ReadLine();

            using (AppDbContext db = new AppDbContext())
            {
                var guild = db.Find<Guild>(id); // Reload
                guild.GuildName = name;
                db.SaveChanges();
            }

            Console.WriteLine("--- Update Complete ---");
            ShowGuilds();
        }

        public static string MakeUpdateJsonStr()
        {
            // JSON Test sample
            var jsonStr = "{\"GuildId:\":1, \"GuildName\":\"ARS\", \"Members\":null}";
            return jsonStr;
        }

        public static void UpdateByFull()
        {
            ShowGuilds();

            // JSON Sample
            //string jsonStr = MakeUpdateJsonStr();
            //var guild = JsonConvert.DeserializeObject<Guild>(jsonStr);

            Guild guild = new Guild()
            {
                GuildId = 1,
                GuildName = "TestGuild"
            };

            using (AppDbContext db = new AppDbContext())
            {
                db.Guilds.Update(guild);
                db.SaveChanges();
            }

            Console.WriteLine("--- Update Complete ---");
            ShowGuilds();
        }

        public static void ShowItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var item in db.items.Include(i=>i.Owner).IgnoreQueryFilters().ToList())
                {
                    if (item.SoftDelete)
                    {
                        Console.WriteLine($"DELETED : ItemId({item.ItemId}) TemplatedId({item.TemplateId}) Owner(0)");
                    }

                    else
                    {
                        if (item.Owner == null)
                            Console.WriteLine($"ItemId({item.ItemId}) TemplatedId({item.TemplateId}) Owner(0)");

                        else
                            Console.WriteLine($"ItemId({item.ItemId}) TemplatedId({item.TemplateId}) OwnerId({item.Owner.PlayerId}) Owner({item.Owner.Name})");
                    }
                }
            }
        }

        public static void Test()
        {
            ShowItems();

            Console.WriteLine("Input Delete PlayerId");
            Console.Write(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext db = new AppDbContext())
            {
                var player = db.Players
                        .Include(p => p.OwnedItem)
                        .Single(p => p.PlayerId == id);

                db.Players.Remove(player);
                db.SaveChanges();
            }

            Console.WriteLine("---Updated---");
            ShowItems();
        }

        // Update Relationship 1v1
        public static void Update_1v1()
        {
            ShowItems();

            Console.WriteLine("Input update PlayerId");
            Console.Write(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext db = new AppDbContext())
            {
                var player = db.Players
                        .Include(p => p.OwnedItem)
                        .Single(p => p.PlayerId == id);

                if (player.OwnedItem != null)
                {
                    player.OwnedItem.TemplateId = 999;
                    player.OwnedItem.CreateDate = DateTime.Now;
                }

                //player.Item = new Item()
                //{
                //    TemplateId = 222,
                //    CreateDate = DateTime.Now,
                //};

                db.SaveChanges();
            }

            Console.WriteLine("---Updated---");
            ShowItems();
        }

        // Update Relationship 1vsMany
        public static void Update_1vM()
        {
            ShowGuilds();

            Console.WriteLine("Input update GuildId");
            Console.Write(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext db = new AppDbContext())
            {
                var guild = db.Guilds
                    //.Include(g => g.Members)
                    .Single(g => g.GuildId == id);

                guild.Members = new List<Player>()
                {
                    new Player() { Name = "HUMS"}
                };

                db.SaveChanges();
            }

            Console.WriteLine("---Updated---");
            ShowGuilds();
        }

        public static void DeleteTest()
        {
            ShowItems();

            Console.WriteLine("Select Delete ItemId");
            Console.Write(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext db = new AppDbContext())
            {
                Item item = db.items.Find(id);
                //db.items.Remove(item);
                item.SoftDelete = true;
                db.SaveChanges();
            }

            Console.WriteLine("---Updated---");
            ShowItems();
        }
    }
}