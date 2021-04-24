using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using profile_service.Cache;
using profile_service.DataAccess;
using profile_service.Interfaces;
using profile_service.Models;
using profile_service.Services;
using StackExchange.Redis;

namespace profile_service
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
            services.AddCors();
            services.AddControllers();

            var redis = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton(s => redis.GetDatabase());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "profile_service", Version = "v1" });
            });

            services.Configure<ProfileDatabaseSettings>(Configuration.GetSection(nameof(ProfileDatabaseSettings)));
            services.AddSingleton<IProfileDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ProfileDatabaseSettings>>().Value);
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserCache, UserCache>();
            services.AddSingleton<IUserDataAccess, UserDataAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "profile_service v1"));
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
