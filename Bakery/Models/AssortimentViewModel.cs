
namespace Bakery.Models;
public class AssortimentViewModel
{
    public List<CategoryVm> Categories { get; set; } = new();
}

public class CategoryVm
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public List<ProductVm> Products { get; set; } = new();
}

public class ProductVm
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string? Description { get; set; }
}