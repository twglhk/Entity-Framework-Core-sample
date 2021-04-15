using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    [Table("TableName")]  // Set TableName
    public class GameResult
    {
        [MaxLength(32)]       // Set String Size
        [Column("ColumName")] // Set ColumnName
        public string UserName { get; set; }

        [NotMapped] // Non DB mapping
        public string Id { get; set; }

        [Key]   // Set PK (Composite key)
        [Column(Order = 0)]  // Set Colum Order
        public int RankingId { get; set; }

        [Key] // Set PK (Composite key)
        [Column(Order = 1)]
        [Required] // not null
        public int SocreId { get; set; }
    }
}
