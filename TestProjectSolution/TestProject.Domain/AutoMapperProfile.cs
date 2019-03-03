using AutoMapper;
using TestProject.Domain.Models;
using TestProject.Entities;

namespace TestProject.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Store --------------------------------------------------------------------------------------------------------------------------
            CreateMap<Store, StoreGetModel>();
            CreateMap<StorePostModel, Store>();
            CreateMap<StoreGetModel, Store>();
            CreateMap<StorePutModel, Store>();

            // StoreManager --------------------------------------------------------------------------------------------------------------------------
            CreateMap<StoreManager, StoreManagerGetModel>();
            CreateMap<StoreManagerPostModel, StoreManager>();
            CreateMap<StoreManagerGetModel, StoreManager>();
            CreateMap<StoreManagerPutModel, StoreManager>();

            // Stock --------------------------------------------------------------------------------------------------------------------------
            CreateMap<Stock, StockGetModel>();
            CreateMap<StockPostModel, Stock>();
            CreateMap<StockGetModel, Stock>();
            CreateMap<StockPutModel, Stock>();
        }
    }
}
