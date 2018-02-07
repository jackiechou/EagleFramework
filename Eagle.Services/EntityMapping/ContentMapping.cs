using AutoMapper;
using Eagle.Entities;
using Eagle.Entities.Contents.Articles;
using Eagle.Entities.Contents.Banners;
using Eagle.Entities.Contents.Media;
using Eagle.Entities.Services.Events;
using Eagle.Services.Dtos;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.EntityMapping.Common;

namespace Eagle.Services.EntityMapping
{
    public class ContentMapping
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Banner, BannerDetail>().ReverseMap();
                cfg.CreateMap<BannerEntry, Banner>().IgnoreUnmapped();
                cfg.CreateMap<BannerPosition, BannerPositionDetail>().ReverseMap();
                cfg.CreateMap<BannerPositionEntry, BannerPosition>().IgnoreUnmapped();
                cfg.CreateMap<BannerScope, BannerScopeDetail>().ReverseMap();
                cfg.CreateMap<BannerType, BannerTypeDetail>().ReverseMap();
                cfg.CreateMap<BannerTypeEntry, BannerType>().IgnoreUnmapped();
                cfg.CreateMap<BannerPage, BannerPageDetail>().ReverseMap();
                cfg.CreateMap<BannerZone, BannerZoneDetail>().ReverseMap();

                cfg.CreateMap<NewsComment, NewsCommentDetail>().IgnoreUnmapped().ReverseMap();
                cfg.CreateMap<NewsCategory, NewsCategoryDetail>().ReverseMap();

                cfg.CreateMap<News, NewsDetail>();
                cfg.CreateMap<NewsInfo, NewsInfoDetail>().IgnoreUnmapped();
                cfg.CreateMap<Event, EventDetail>().IgnoreUnmapped();

                cfg.CreateMap<MediaType, MediaTypeDetail>().IncludeBase<BaseEntity, BaseDto>();
                cfg.CreateMap<MediaAlbum, MediaAlbumDetail>().IncludeBase<BaseEntity, BaseDto>();
                
            });
        }
    }
}
