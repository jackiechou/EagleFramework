using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductFile")]
    public class ProductFile : EntityBase
    {
        public ProductFile()
        {
            IsImage = true;
            Width = ImageSettings.ImageWidthCga;
            Height = ImageSettings.ImageHeightCga;
            ThumbWidth = ImageSettings.ImageWidthVga;
            ThumbHeight = ImageSettings.ImageHeightVga;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
        public int ProductId { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
        public string FileExtension { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public ProductFileStatus? Status { get; set; }

        public bool? IsImage { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }

        public int VendorId { get; set; }
    }
}
