using AutoMapper;
using WebRozetka.Data;
using WebRozetka.Data.Entities;
using WebRozetka.Data.Entities.Addres;
using WebRozetka.Data.Entities.Orders;
using WebRozetka.Models.Category;
using WebRozetka.Models.NovaPoshta;
using WebRozetka.Models.Orders;
using WebRozetka.Models.Product;

namespace WebRozetka.Mapper
{
    public class AppMapProfile : Profile
    {
        private readonly AppEFContext _context;
        public AppMapProfile(AppEFContext context)
        {
            _context = context;

            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryCreateViewModel, CategoryEntity>();
         
            CreateMap<ProductCreateViewModel, ProductEntity>()
                .ForMember(x=>x.ProductImages, opt=>opt.Ignore());

            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x=>x.Category.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x=>x.ProductImages.Select(x=>x.Name)));

            CreateMap<BasketEntity, BasketItemViewModel>()
                .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.Product.Name))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Product.Price));

            CreateMap<NPAreaItemViewModel, AreaEntity>();

            CreateMap<NPSettlementItemViewModel, SettlementEntity>()
                .ForMember(dest => dest.AreaId, opt =>  opt.MapFrom(src => _context.Areas.Where(x => x.Ref == src.Area).Select(x => x.Id).SingleOrDefault()))
                .ForMember(dest => dest.Area, opt => opt.Ignore());

            CreateMap<NPWarehouseItemViewModel, WarehouseEntity>()
                .ForMember(dest => dest.SettlementId, opt => opt.MapFrom(src => _context.Settlements.Where(x => x.Ref == src.SettlementRef).Select(x => x.Id).SingleOrDefault()))
                .ForMember(dest => dest.Settlement, opt => opt.Ignore());

        }
    }
}
