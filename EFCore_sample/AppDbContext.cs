using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    class AppDbContext : DbContext
    {
        public DbSet<Item> items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        // DB Connection string
        // set which DB connected (option, authorization)
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Model level filtering
            builder.Entity<Item>().HasQueryFilter(i => i.SoftDelete == false);

            // Create Index
            builder.Entity<Player>()
                .HasIndex(p => p.Name)
                .HasDatabaseName("Index_Person_Name")
                .IsUnique();

            // Build Relationship : one to many
            builder.Entity<Player>()
                .HasMany(p => p.CreatedItems)
                .WithOne(i => i.Creator)
                .HasForeignKey(i => i.TestCreatorId);

            // Build Relationship : one to one
            builder.Entity<Player>()
                .HasOne(p => p.OwnedItem)
                .WithOne(i => i.Owner)
                .HasForeignKey<Item>(i => i.TestOwnerId);

            // Shadow Property
            builder.Entity<Item>().Property<DateTime>("RecoveredDate");

            // Backing Field Mapping
            builder.Entity<Item>()
                .Property(i => i.JsonData)
                .HasField("_jsonData");

            /* Fluet API Sample
            
            builder.Entity<GameResult>()
              .ToTable("GameTableName");    // Set table name

            builder.Entity<GameResult>()
              .Property(g => g.RankingId)
              .HasColumnName("Ranking ID"); // set column name

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .IsUnicode(false);            // set Varchar. true == nVarchar

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .HasMaxLength(123);           // Set String Size

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .IsRequired();                // set not null

            builder.Entity<GameResult>()
              .HasIndex(x => x.UserName);   // set index

            builder.Entity<GameResult>()
              .HasQueryFilter(p => p.SocreId > 1000); // set filter

            builder.Entity<GameResult>()
              .Ignore(g => g.Id);           // non mapping

            builder.Entity<GameResult>()
              .HasKey(x => new { x.RankingId, x.SocreId }); // Set PK (Composite key)

            builder.Entity<GameResult>()
              .HasIndex(x => new { x.RankingId, x.SocreId }); // Set index (Composit Index)

            builder.Entity<GameResult>()
              .HasIndex(g => g.RankingId)
              .HasName("ASD") // set Index name
              .IsUnique();	  // set UNIQUE Index

            */
        }
    }
}
