using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Articles
{
    [Table("NewsComment")]
    public class NewsComment : EntityBase
    {
        public NewsComment()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string CommentName { get; set; }
        public string CommentText { get; set; }
        public string CreatedByEmail { get; set; }
        public bool? IsReplied { get; set; }
        public NewsCommentStatus? IsPublished { get; set; }
        public string Ip { get; set; }
        public DateTime? CreatedDate { get; set; }


        public int NewsId { get; set; }

        //[ForeignKey("NewsId")]
        //public virtual News News { get; set; }
    }
}
