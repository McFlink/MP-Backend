using Microsoft.AspNetCore.Identity;

namespace MP_Backend.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Retailer", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var identityRole = new IdentityRole(role);
                    await roleManager.CreateAsync(identityRole);
                }
            }
        }
    }
}
