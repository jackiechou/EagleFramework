using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum FileLocation
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Upload", Description = "Upload", Order = 0)]
        Upload =1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Document", Description = "Document", Order = 1)]
        Document =2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Images", Description = "Images", Order = 2)]
        Images =3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Banner", Description = "Banner", Order = 3)]
        Banner =4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Menu", Description = "Menu", Order = 4)]
        Menu =5,
        [Display(ResourceType = typeof(LanguageResource), Name = "User", Description = "User", Order = 5)]
        User =6,
        [Display(ResourceType = typeof(LanguageResource), Name = "News", Description = "News", Order = 6)]
        News =7,
        [Display(ResourceType = typeof(LanguageResource), Name = "Event", Description = "Event", Order = 7)]
        Event =8,
        [Display(ResourceType = typeof(LanguageResource), Name = "Product", Description = "Product", Order = 8)]
        Product =9,
        [Display(ResourceType = typeof(LanguageResource), Name = "Media", Description = "Media", Order = 9)]
        Media =10,
        [Display(ResourceType = typeof(LanguageResource), Name = "Audio", Description = "Audio", Order = 10)]
        Audio =11,
        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundAudio", Description = "BackgroundAudio", Order = 11)]
        BackgroundAudio =12,
        [Display(ResourceType = typeof(LanguageResource), Name = "Music", Description = "Music", Order = 12)]
        Music =13,
        [Display(ResourceType = typeof(LanguageResource), Name = "Video", Description = "Video", Order = 13)]
        Video =14,
        [Display(ResourceType = typeof(LanguageResource), Name = "VideoThumbnail", Description = "VideoThumbnail", Order = 14)]
        VideoThumbnail =15,
        [Display(ResourceType = typeof(LanguageResource), Name = "Company", Description = "Company", Order = 15)]
        Company = 16,
        [Display(ResourceType = typeof(LanguageResource), Name = "Gallery", Description = "Gallery", Order = 16)]
        Gallery = 17,
        [Display(ResourceType = typeof(LanguageResource), Name = "Vendor", Description = "Vendor", Order = 17)]
        Vendor = 18,
        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundPhoto", Description = "BackgroundPhoto", Order = 18)]
        BackgroundPhoto = 19,
        [Display(ResourceType = typeof(LanguageResource), Name = "Partner", Description = "Partner", Order = 19)]
        Partner= 20,
        [Display(ResourceType = typeof(LanguageResource), Name = "Qualification", Description = "Qualification", Order = 20)]
        Qualification = 21,
        [Display(ResourceType = typeof(LanguageResource), Name = "ServicePack", Description = "ServicePack", Order = 21)]
        ServicePack = 22,
        [Display(ResourceType = typeof(LanguageResource), Name = "Manufacturer", Description = "Manufacturer", Order = 22)]
        Manufacturer = 23,
        [Display(ResourceType = typeof(LanguageResource), Name = "Employee", Description = "Employee", Order = 23)]
        Employee = 24,
        [Display(ResourceType = typeof(LanguageResource), Name = "Album", Description = "Album", Order = 24)]
        Album = 25,
        [Display(ResourceType = typeof(LanguageResource), Name = "PlayList", Description = "PlayList", Order = 25)]
        PlayList = 26,
    }
}
