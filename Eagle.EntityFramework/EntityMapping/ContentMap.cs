using System.Data.Entity;
using Eagle.Entities.Contents.Articles;
using Eagle.Entities.Contents.Banners;
using Eagle.Entities.Contents.Documents;
using Eagle.Entities.Contents.Feedbacks;
using Eagle.Entities.Contents.Galleries;
using Eagle.Entities.Contents.Media;
using Eagle.Entities.Contents.Tags;

namespace Eagle.EntityFramework.EntityMapping
{
    public static class ContentMap
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().ToTable("News");
            modelBuilder.Entity<NewsCategory>().ToTable("NewsCategory");
            modelBuilder.Entity<NewsComment>().ToTable("NewsComment");
            modelBuilder.Entity<NewsRating>().ToTable("NewsRating");

            modelBuilder.Entity<Feedback>().ToTable("Feedback");

            modelBuilder.Entity<Banner>().ToTable("Banner");
            modelBuilder.Entity<BannerType>().ToTable("BannerType");
            modelBuilder.Entity<BannerPosition>().ToTable("BannerPosition");
            modelBuilder.Entity<BannerScope>().ToTable("BannerScope");
            modelBuilder.Entity<BannerPage>().ToTable("BannerPage").HasKey(s => new { s.BannerId, s.PageId });
            modelBuilder.Entity<BannerZone>().ToTable("BannerZone").HasKey(s => new { s.BannerId, s.PositionId });

            modelBuilder.Entity<GalleryTopic>().ToTable("GalleryTopic");
            modelBuilder.Entity<GalleryCollection>().ToTable("GalleryCollection");
            modelBuilder.Entity<GalleryFile>().ToTable("GalleryFile");
            
            modelBuilder.Entity<MediaAlbum>().ToTable("Media.MediaAlbum");
            modelBuilder.Entity<MediaAlbumFile>().ToTable("Media.MediaAlbumFile");
            modelBuilder.Entity<MediaArtist>().ToTable("Media.MediaArtist");
            modelBuilder.Entity<MediaComposer>().ToTable("Media.MediaComposer");
            modelBuilder.Entity<MediaFile>().ToTable("Media.MediaFile");
            modelBuilder.Entity<MediaPlayList>().ToTable("Media.MediaPlayList");
            modelBuilder.Entity<MediaPlayListFile>().ToTable("Media.MediaPlayListFile");
            modelBuilder.Entity<MediaTopic>().ToTable("Media.MediaTopic");
            modelBuilder.Entity<MediaType>().ToTable("Media.MediaType");

            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<TagIntegration>().ToTable("TagIntegration");

            modelBuilder.Entity<Documentation>().ToTable("Documentation");
        }
    }
}