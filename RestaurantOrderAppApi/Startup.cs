using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestaurantOrderApp.Application.Dto;
using RestaurantOrderApp.Application.IServices;
using RestaurantOrderApp.Application.Services;
using RestaurantOrderApp.Data.Context;
using RestaurantOrderApp.Data.Repositories;
using RestaurantOrderApp.Domain.IRepositories;
using RestaurantOrderApp.Domain.Models;

namespace RestaurantOrderAppApi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantOrderAppApi", Version = "v1" });
            });

            services.AddScoped<RestaurantOrderContext>();
            services.AddScoped<IClientOrderHistoryRepository, ClientOrderHistoryRepository>();
            services.AddScoped<IRestaurantOrderAppService, RestaurantOrderAppService>();

            services.AddDbContext<RestaurantOrderContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RestaurantOrderConnection")));

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientOrderHistory, ClientOrderHistoryDto>();
                cfg.CreateMap<ClientOrderHistoryDto, ClientOrderHistory>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddCors();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("https://127.0.0.1:4200", "http://127.0.0.1:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantOrderAppApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(option => option.AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(options => options.AllowAnyOrigin());
        }
    }
}
