using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    //Entity class, DB table name = Item
    [Table("Item")]
    public class Item
    {
        public bool SoftDelete { get; set; }

        public int ItemId { get; set; } // PK
        public int TemplateId { get; set; } // Item Id
        public DateTime CreateDate { get; set; }

        // Other class ref -> FK (Navigational Property)
        //[ForeignKey("OwnerID")] => Defalut : non-nullable
        public int TestOwnerId { get; set; }
        public Player Owner { get; set; }

        public int? TestCreatorId { get; set; }
        public Player Creator { get; set; }
    }

    //Entity class, DB table name = Player
    [Table("Player")]
    public class Player
    {
        public int PlayerId { get; set; }   // DB table Primary key
                                            // name convention (Class + Id)

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }    // Alternate Key

        [InverseProperty("Owner")]
        public Item OwnedItem { get; set; }

        [InverseProperty("Creator")]
        public ICollection<Item> CreatedItems { get; set; }

        public Guild Guild { get; set; }
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
}