using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PlayRoulette.API.Data;
using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Helpers;
using System.Text;

namespace PlayRoulette.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<DataContext>();
            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString(Constants.ConnectionName));
            });
            services
              .AddAuthentication()
              .AddCookie()
              .AddJwtBearer(cfg =>
              {
                  cfg.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidIssuer = Configuration[Constants.TokensIssuer],
                      ValidAudience = Configuration[Constants.TokensAudience],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[Constants.TokensKey]))
                  };
              });
            services.AddTransient<SeedDb>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IRouletteHelper, RouletteHelper>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
