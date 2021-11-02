using AutoMapper;
using JustTradeIt.Software.API.Models.Helpers;
using JustTradeIt.Software.API.Models.Profiles;
using JustTradeIt.Software.API.Repositories;
using JustTradeIt.Software.API.Repositories.Implementations;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Implementations;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace JustTradeIt.Software.API
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
            services.AddEntityFrameworkSqlite().AddDbContext<JustTradeItContext>();
            services.AddDbContext<JustTradeItContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddHttpClient();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITradeRepository, TradeRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<AccountService, AccountService>();
            services.AddTransient<ItemService, ItemService>();
            services.AddTransient<UserService, UserService>();
            services.AddTransient<TradeService, TradeService>();
            services.AddTransient<TokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new ImageProfile());
                cfg.AddProfile(new ItemDetailProfile());
                cfg.AddProfile(new ItemProfile());
                cfg.AddProfile(new TradeProfile());
                cfg.AddProfile(new TradeDetailsProfile());
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddCors();
            services.AddSingleton<IMapper>(sp => config.CreateMapper());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JustTradeIt.Software.API", Version = "v1" });
            });
            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,JustTradeItContext context)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JustTradeIt.Software.API v1"));
            }

            app.UseHttpsRedirection();
            context.Database.EnsureCreated();


            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
