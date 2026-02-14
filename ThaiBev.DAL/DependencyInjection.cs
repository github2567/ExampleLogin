using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using ThaiBev.DAL.Common;
using ThaiBev.DAL.Data;
using ThaiBev.Domain.Models;

namespace ThaiBev.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext registrations
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));

            services.AddDbContext<ThaiBevDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ThaiBevDbContext).Assembly.FullName)
                ));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequiredLength = 8;
            })
            .AddPasswordValidator<NoThaiCharactersPasswordValidator<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
                
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Index";
            });


            services.AddScoped<UserListDAL>();


            return services;
        }

        public class NoThaiCharactersPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
        {
            public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
            {
                if (!string.IsNullOrEmpty(password))
                {
                    if (Regex.IsMatch(password, @"[\u0E00-\u0E7F]"))
                    {
                        var error = new IdentityError
                        {
                            Code = "ThaiCharactersNotAllowed",
                            Description = "Password must not contain Thai characters."
                        };
                        return Task.FromResult(IdentityResult.Failed(error));
                    }
                }

                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
