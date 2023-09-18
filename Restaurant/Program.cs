using Restaurant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Restaurant.Services;

var builder = WebApplication.CreateBuilder(args);



ConfigureServices(builder.Services, builder.Configuration);



var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});


app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddTransient<BasketHelper>();

    services.AddAuthentication( o =>
    {
        o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       
    }).AddCookie((o) =>
    {
        o.LoginPath = "/User/ShowLogin";
        //trying find out why adding accountctronler breaking it
        
    });
    services.AddMvc();

    services.AddRazorPages().AddRazorRuntimeCompilation();

    string con = configuration.GetConnectionString("BloggingDatabase");
    services.AddDbContext<RestaurantContext>(options =>
         options.UseSqlServer(con));


}
