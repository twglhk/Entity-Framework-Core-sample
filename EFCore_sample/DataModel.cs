using System;
using System.Collections.Generic;
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
        public int ItemId { get; set; } // PK
        public int TemplateId { get; set; } // Item Id
        public DateTime CreateDate { get; set; }

        // Other class ref -> FK (Navigational Property)
        public int OwnerId { get; set; }
        public Player Owner { get; set; }
    }

    //Entity class, DB table name = Player
    public class Player
    {
        public int PlayerId { get; set; }   // DB table Primary key
                                            // name convention (Class + Id)
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}