using AutoMapper;
using Eagle.Entities.Services.Booking;
using Eagle.Services.Dtos.Services.Booking;

namespace Eagle.Services.EntityMapping
{
    public class ServiceMapping
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ServiceDiscount, ServiceDiscountDetail>();
                cfg.CreateMap<ServicePackDuration, ServicePackDurationDetail>();
                cfg.CreateMap<ServicePackProvider, ServicePackProviderDetail>();
                
                cfg.CreateMap<ServicePack, ServicePackDetail>();
                cfg.CreateMap<ServicePeriod, ServicePeriodDetail>();
                
            });
        }
    }
}