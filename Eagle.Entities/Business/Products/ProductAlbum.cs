using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductAlbum")]
    public class ProductAlbum : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlbumId { get; set; }
        public int ProductId { get; set; }
        public int FileId { get; set; }
    }
}
