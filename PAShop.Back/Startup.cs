﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;
using static Model.Models.Role;

namespace PAShop.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //addScoped indicate what dependency to create
        public void ConfigureServices(IServiceCollection services) {

            services.AddCors(options =>
            {
                options.AddPolicy("CORS", b =>
                {
                    b.AllowAnyHeader();
                    b.AllowAnyMethod();
                    b.AllowAnyOrigin();
                });
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddSessionStateTempDataProvider()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }); ;

            services.AddSession();
            services.AddDbContext<PAShopDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("PAShopDb")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<DbContext, PAShopDbContext>();

            services.AddScoped<IGenericRepository<Basket>, GenericRepository<Basket>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Inventory>, GenericRepository<Inventory>>();
            services.AddScoped<IGenericRepository<Item>, GenericRepository<Item>>();
            services.AddScoped<IGenericRepository<StockMovement>, GenericRepository<StockMovement>>();
            services.AddScoped<IGenericRepository<Transaction>, GenericRepository<Transaction>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
          //  services.AddScoped<IGenericRepository<BasketItem>, GenericRepository<BasketItem>>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGenericService<User>, UserService>();
            services.AddScoped<IGenericService<Basket>, BasketService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IGenericService<Item>, ItemService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IGenericService<Inventory>, GenericService<Inventory>>();
            services.AddScoped<IGenericService<StockMovement>, GenericService<StockMovement>>();
            services.AddScoped<IGenericService<Transaction>, GenericService<Transaction>>();
            //services.AddScoped<IGenericService<BasketItem>, GenericService<BasketItem>>();
            SetupAuth(services);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
            app.UseCors(builder =>
                builder.WithOrigins("https://localhost:44336/")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

        }


        private void SetupAuth(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireAssertion(ctx =>
                {
                    return ctx.User.HasClaim(ClaimTypes.Role, Role.LoggedUser.ToString()) ||
                           ctx.User.HasClaim(ClaimTypes.Role, Role.Vendor.ToString()) ||
                           ctx.User.HasClaim(ClaimTypes.Role, Role.Admin.ToString());
                }));

                options.AddPolicy("Vendor", policy => policy.RequireAssertion(ctx =>
                {
                    return ctx.User.HasClaim(ClaimTypes.Role, Role.Vendor.ToString()) ||
                           ctx.User.HasClaim(ClaimTypes.Role, Role.Admin.ToString());
                }));
                options.AddPolicy("Admin", policy => policy.RequireAssertion(ctx =>
                {
                    return ctx.User.HasClaim(ClaimTypes.Role, Role.Admin.ToString());
                }));
            });
        }
    }
}
