using System.ComponentModel.DataAnnotations;

namespace Eagle.Entities.Common
{
    public class ImageModel : CommonEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[Required]
        //public string Title { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
