using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductVote")]
    public class ProductVote : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }
        public string Votes { get; set; }
        public int AllVotes { get; set; }
        public int Rating { get; set; }
        public string LastIp { get; set; }

        public int ProductId { get; set; }
    }
}
