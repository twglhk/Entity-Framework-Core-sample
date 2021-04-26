using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    public class ItemOption
    {
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Hp { get; set; }
    }

    public class ItemDetail
    {
        public int ItemDetailId { get; set; }
        public string Description { get; set; }
    }

    public class ItemReview
    {
        public int ItemReviewId { get; set; }
        public int Score { get; set; }
    }

    public enum ItemType
    {
        NormalItem,
        EventItem
    }

    //Entity class, DB table name = Item
    [Table("Item")]
    public class Item
    {
        public ItemType Type { get; set; }

        // Backing Field
        private string _jsonData;
        public string JsonData 
        {
            get { return _jsonData; }
        }

        //public void SetOption(ItemOption option)
        //{
        //    _jsonData = JsonConvert.SerializeObject(option);
        //}

        //public ItemOption GetOption()
        //{
        //    return JsonConvert.DeserializeObject<ItemOption>(_jsonData);
        //}

        // Owned Type
        public ItemOption Option { get; set; }

        // Table Splitting
        public ItemDetail Detail { get; set; }

        // Backing Property & Relationship
        //public double? AverageScore { get; set; }
        //private readonly List<ItemReview> _reviews = new List<ItemReview>();
        //public IEnumerable<ItemReview> Reviews { get { return _reviews.ToList(); } }

        //public void AddReview(ItemReview review)
        //{
        //    _reviews.Add(review);
        //    AverageScore = _reviews.Average(r => r.Score);
        //}

        //public void RemoveReview(ItemReview review)
        //{
        //    _reviews.Remove(review);
        //    AverageScore = _reviews.Any() ? _reviews.Average(r => r.Score) : (double?)null;
        //}

        // UDF
        public ICollection<ItemReview> Reviews { get; set; }

        // Migration Test
        public int ItemGrade { get; set; }

        public bool SoftDelete { get; set; }
        public int ItemId { get; set; } // PK
        public int TemplateId { get; set; } // Item Id
        public DateTime CreateDate { get; private set; } //= new DateTime(2020, 1, 1); // Default Value

        // Other class ref -> FK (Navigational Property)
        //[ForeignKey("OwnerID")] => Defalut : non-nullable
        public int? TestOwnerId { get; set; }
        public Player Owner { get; set; }

        //public int? TestCreatorId { get; set; }
        //public Player Creator { get; set; }
    }

    // Entity for TPH
    public class EventItem : Item
    {
        public DateTime DestroyDate { get; set; }
    }

    public class PlayerNameGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            string name = $"Player_{DateTime.Now.ToString("yyyyMMdd")}";
            return name;
        }
    }

    // Change Tracker Test
    public interface ILogEntity
    {
        DateTime CreateTime { get; }
        void SetCreateTime();
    }

    //Entity class, DB table name = Player
    [Table("Player")]
    public class Player : ILogEntity
    {
        public int PlayerId { get; set; }   // DB table Primary key
                                            // name convention (Class + Id)

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }    // Alternate Key

        [InverseProperty("Owner")]
        public Item OwnedItem { get; set; }

        //[InverseProperty("Creator")]
        //public ICollection<Item> CreatedItems { get; set; }

        public int? GuildId { get; set; }
        public Guild Guild { get; set; }

        public DateTime CreateTime { get; private set; }

        public void SetCreateTime()
        {
            CreateTime = DateTime.Now;
        }
    }

    [Table("Guild")]
    public class Guild
    {
        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }
    }

    // DTO (Data Transfer Object)
    public class GuildDto
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }

    public class Monster
    {
        public int MonsterId { get; set; }
        public string Name { get; set; }
        //public ICollection<Reward> Reward {get; set;}
    }

    public class Reward
    {
        public int RewardId { get; set; }
        public int MonsterId { get; set; }
        public Monster Monster { get; set; }
    }
}