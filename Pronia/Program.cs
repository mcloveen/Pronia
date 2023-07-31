using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess.Pronia.DataAccess;
using Pronia.ExtensionServices.Implements;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Services.Implements;
using Pronia.Services.Interfaces;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISliderService, SliderService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddDbContext<ProniaDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration["Server=MacDesktop;Database=Pronia;Username=sa;Password=said1234@"]);
                //opt.UseNpgsql();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}"
                );
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}