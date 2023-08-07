using Pronia.ExtensionServices.Implements;
using Pronia.ExtensionServices.Interfaces;
using P137Pronia.Services.Interfaces;
using Pronia.Services.Implements;


namespace Pronia.Services
{
    public static class ServiceRegistration
	{
		public static void AddServices(this IServiceCollection services)
		{
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<LayoutService>();
        }
	}
}

