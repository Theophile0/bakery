using AutoMapper;
using Bakery.Domains.Data.Entities;
using Bakery.Models;
using System.Globalization;
namespace Bakery.AutoMapper
{
    public class AssortimentProfile: Profile
    {
        public AssortimentProfile() 
        { 
            CreateMap<Product, ProductVm>()
                .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => FormatEuro(src.Price))
            );

            CreateMap<ProductCategory, CategoryVm>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => $"cat-{src.Id}")
            )
            .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(src =>
                    src.Products
                        .OrderBy(p => p.SortOrder)
                )
            );

            CreateMap<IReadOnlyList<ProductCategory>, AssortimentViewModel>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src)
            );
            CreateMap<IEnumerable<ProductCategory>, AssortimentViewModel>()
                    .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.OrderBy(c => c.SortOrder)));
        }


        private static string FormatEuro(decimal price)
        {
            var culture = CultureInfo.GetCultureInfo("nl-BE");
            return "€" + price.ToString("0.00", culture);
        }
    }
}
