using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Service;
using API.ServiceInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<Settings>(options =>
            {
                options.JwtKey = Configuration.GetSection("Jwt:JwtKey").Value;
                options.JwtIssuer = Configuration.GetSection("Jwt:JwtIssuer").Value;
                options.JwtExpireMinutes = int.Parse(Configuration.GetSection("Jwt:JwtExpireMinutes").Value);
            });
            services.AddDbContextPool<DBContext>(x => x.UseSqlServer(Configuration.GetConnectionString("CarSellerDB")));
           

            services.AddTransient<ICarOfferService, CarOfferService>();
            services.AddTransient<ICarModelService, CarModelService>();
            services.AddTransient<ICarProducentService, CarProducentService>();
            services.AddTransient<IFuelTypeService, FuelTypeService>();
            services.AddTransient<IEquipmentService, EquipmentService>();
            services.AddTransient<ISearchOfferService, SearchOfferService>();
            services.AddTransient<IAccountService, AccountService>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddIdentity<User, Role>()
             .AddEntityFrameworkStores<DBContext>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var a = Configuration.GetSection("Jwt:JwtIssuer").Value;
            services.AddAuthentication(x =>{
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
                 
            .AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("Jwt:JwtIssuer").Value,
                    ValidAudience = Configuration.GetSection("Jwt:JwtIssuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:JwtKey").Value)),
                    RoleClaimType = "role"
                };
            });

         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            //     app.UseHttpsRedirection();  
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseDefaultFiles();

    
            app.UseMvc();
        }
    }
}
