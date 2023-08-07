using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Services;

var builder = WebApplication.CreateBuilder(args);

object value = builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); 

builder.Services.AddServices();
builder.Services.AddSession();

builder.Services.AddDbContext<ProniaDBContext>(opt => {
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:MSSQL"]);
}); 

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts();
}

if(app.Environment.IsProduction())
{
    app.UseStatusCodePagesWithRedirects("~/error.html");
} 

app.UseSession();
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

