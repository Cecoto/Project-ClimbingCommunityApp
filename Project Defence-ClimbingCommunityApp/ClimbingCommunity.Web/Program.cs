using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ClimbingCommunity.Data;
using ClimbingCommunity.Data.Models;
using ClimbingCommunity.Services.Contracts;
using ClimbingCommunity.Web.Infrastructure.Extensions;
using ClimbingCommunity.Web.Infrastructure.ModelBinders;
using WebShopDemo.Core.Data.Common;



WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

IConfiguration configuration = builder.Configuration.GetSection("AdminSettings");

builder.Services.AddDbContext<ClimbingCommunityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");

    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");

    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");

    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");

    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ClimbingCommunityDbContext>();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddApplicationServices(typeof(IUserService));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccess", policy =>
    {
        policy.RequireRole("Administrator");
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
    options.AccessDeniedPath = "/Home/Error/401"; // need to create 401 page!!
});

builder.Services
    .AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    });

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.SeedRoles();
app.AddAdministrator(configuration["AdminEmail"], configuration["AdminPassword"]);

app.EnableOnlineUsersCheck();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "/{controller=Home}/{action=Index}/{id?}"
        );
    endpoints.MapRazorPages();
});

app.Run();
