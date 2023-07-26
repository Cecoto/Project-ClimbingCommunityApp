namespace ClimbingCommunity.Web.Infrastructure.Extensions
{
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

        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>()!;

            Task.Run(async () =>
            {
                if (!roleManager.RoleExistsAsync(Climber).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(Climber));
                }

                if (!roleManager.RoleExistsAsync(Coach).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(Coach));
                }

                if (!roleManager.RoleExistsAsync(Administrator).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(Administrator));
                }
            })
                .GetAwaiter()
                .GetResult();
            return app;
        }
    }
}
