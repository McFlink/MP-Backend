using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MP_Backend.Data;
using MP_Backend.Data.Repositories.Products;
using MP_Backend.Infrastructure.Data;
using MP_Backend.Infrastructure.Identity;
using MP_Backend.Infrastructure.Middleware;
using MP_Backend.Services.Auth;
using MP_Backend.Services.Email;
using MP_Backend.Services.Products;
using System.Text;
using Serilog;
using System.Threading.Tasks;
using MP_Backend.Services.Orders;
using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Services.UserServices;
using MP_Backend.Data.Repositories.Users;
using System.Security.Claims;
using MP_Backend.Data.Repositories.ProductVariants;

namespace MP_Backend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Base services
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Base repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>(); // Dont need? Put logic in UserRepo?

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAppEmailSender, SendGridEmailSender>();

            // Email verification
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            });


            var jwtSettings = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)
                    ),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header. Example: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeeder.SeedAsync(services);
                await ProductSeeder.SeedAsync(services);
            }
            
            app.Run();
        }
    }
}
