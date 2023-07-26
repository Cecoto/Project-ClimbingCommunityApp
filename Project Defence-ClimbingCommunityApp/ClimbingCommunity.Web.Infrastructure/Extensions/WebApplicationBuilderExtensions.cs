namespace ClimbingCommunity.Web.Infrastructure.Extensions
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    using static Common.RoleConstants;

    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// This method register all services with thier interfaces and implemtation with given assembly.The assembly is taken from the type of the random service interface or implementation provider.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided.");
            }

            Type[] servicesTypes = serviceAssembly.GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (var implementationType in servicesTypes)
            {
                Type? interfaceType = implementationType
                    .GetInterface($"I{implementationType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException($"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
            }
        }
        /// <summary>
        /// This method seed roles in the application
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>()!;

            Task.Run(async () =>
            {
                if (!roleManager.RoleExistsAsync(RoleConstants.Climber).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.Climber));
                }

                if (!roleManager.RoleExistsAsync(RoleConstants.Coach).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.Coach));
                }

                if (!roleManager.RoleExistsAsync(Administrator).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));
                }
            })
                .GetAwaiter()
                .GetResult();
            return app;
        }

        /// <summary>
        /// This method create application user who is the administrator of the app.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddAdministrator(this IApplicationBuilder app, string email, string password)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<ApplicationUser> userManager = serviceProvider.GetService<UserManager<ApplicationUser>>()!;

            Task.Run(async () =>
            {
                ApplicationUser adminUser = await userManager.FindByNameAsync(email);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        Email = email,
                        UserName = email,
                        UserType = Administrator,
                        FirstName = "Admin",
                        LastName = ""

                    };
                }
                await userManager.CreateAsync(adminUser, password);

                await userManager.AddToRoleAsync(adminUser, Administrator);
            })
                .GetAwaiter()
                .GetResult();

            return app;

        }

    }
}
