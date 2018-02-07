using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductComment")]
    public class ProductComment : EntityBase
    {
        public ProductComment()
        {
            IsReplied = false;
            IsActive = ProductCommentStatus.Active;
        }
        [NotMapped]
        public int Id => CommentId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string CommentName { get; set; }
        public string CommentEmail { get; set; }
        public string CommentMobile { get; set; }
        public string CommentText { get; set; }
        public bool? IsReplied { get; set; }
        public ProductCommentStatus IsActive { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
