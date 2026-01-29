using AutoMapper;
using Bakery.Domains.Data.Entities;
using Bakery.Models;
using Bakery.Repositories.Interfaces;
using Bakery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Bakery.Controllers
{
    public class AssortimentController : Controller
    {
        private readonly IProductService _productservice;
        private readonly IMapper _mapper;
        

        public AssortimentController(IProductService productService, IMapper mapper) 
        { 
            _productservice = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            try
            {
                var products = await _productservice.GetProductsForPublicAsync();
                var categories = await _productservice.GetProductCategoriesAsync();

                var assortimentViewModel = new AssortimentViewModel();

                foreach (var category in categories)
                {
                    var productsVm = products
               .Where(p => p.IsActive && p.CategoryId == category.Id)   
               .OrderBy(p => p.SortOrder)
               .Select(p => new ProductVm
               {
                   Name = p.Name,
                   Price = FormatEuro(p.Price),
                   Description = p.Description
               })
               .ToList();

                    if (productsVm.Count == 0) continue;

                    var categoryVm = new CategoryVm
                    {
                        Id = $"cat-{category.Id}",
                        Name = category.Name,
                        Products = productsVm
                    };
                    assortimentViewModel.Categories.Add(categoryVm);
                }

                return View(assortimentViewModel);

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString());
            }

            return View();
        }

        private static string FormatEuro(decimal price)
        {
            var culture = CultureInfo.GetCultureInfo("nl-BE");
            return "€" + price.ToString("0.00", culture);
        }
    }
}
