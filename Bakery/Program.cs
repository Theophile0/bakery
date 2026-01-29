using Bakery.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bakery.Repositories.Interfaces;
using Bakery.Repositories.Implementation;
using Bakery.Services.Interfaces;
using Bakery.Services.Implementation;
using AutoMapper;
using Bakery.AutoMapper;




var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(cfg => { }, typeof(AssortimentProfile));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IOpeningHoursRepository, OpeningHoursRepository>();
builder.Services.AddScoped<ISpecialDayRepository, SpecialDayRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOpeningHoursService, OpeningHoursService>();
builder.Services.AddScoped<ISpecialDayService, SpecialDayService>();



var app = builder.Build();


var mapper = app.Services.GetRequiredService<AutoMapper.IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
