using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductAlbumDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumId")]
        public int AlbumId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileContent")]
        public string FileContent { get; set; }
    }

    public class ProductAlbumEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FileId { get; set; }
    }

    public class ProductAlbumEditEntry : ProductAlbumEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumId")]
        public int AlbumId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }

}
