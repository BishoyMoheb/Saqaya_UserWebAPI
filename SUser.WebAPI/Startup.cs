using Microsoft.AspNetCore.Authentication.JwtBearer;//To use AddJwtBearer
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;//To use UseSqlServer
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SUser.CLDataAccess.EFContext;//To use MUserDbContext
using SUser.CLDataAccess.RepositoryPattern;//To use Repository Pattern files
using SUser.CLDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration config_i)
        {
            ConfigI = config_i;
        }


        public IConfiguration ConfigI { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection SerCollectionI)
        {
            SerCollectionI.AddControllers();

            // Registering MUserDbContext with the ASP.NET Core dependency injection system
            SerCollectionI.AddDbContext<MUserDbContext>
                (DbCOBuilder => DbCOBuilder.UseSqlServer(ConfigI.GetConnectionString("DBConn_SUserAPI")));

            // Registering the Repository Pattern
            SerCollectionI.AddTransient<IUserRespository, UserSQL_Repository>();

            // Registering IdUserExtension with MUserDbContext
            SerCollectionI.AddIdentity<IdUserExtension, IdentityRole>()
                .AddEntityFrameworkStores<MUserDbContext>();

            // Add JWT Configuartion variables
            var jwtIssuer = SerCollectionI.Configure<string>(ConfigI.GetSection("Jwt:Issuer"));
            var jwtKey = SerCollectionI.Configure<string>(ConfigI.GetSection("Jwt:Key"));

            // Add JWT Bearer Options
            SerCollectionI.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                          .AddJwtBearer(JWTBOptions =>
                          {
                              JWTBOptions.TokenValidationParameters = new TokenValidationParameters
                              {
                                  ValidateIssuer = true,
                                  ValidateAudience = true,
                                  ValidateLifetime = true,
                                  ValidateIssuerSigningKey = true,
                                  ValidIssuer = jwtIssuer.ToString(),
                                  ValidAudience = jwtIssuer.ToString(),
                                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey.ToString()))
                              };
                          });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder AppBuiderI, IWebHostEnvironment WHEnvI)
        {
            if (WHEnvI.IsDevelopment())
            {
                AppBuiderI.UseDeveloperExceptionPage();
            }

            AppBuiderI.UseRouting();

            AppBuiderI.UseAuthorization();

            AppBuiderI.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
