using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UdemyRestfulAPICourse.Data;

namespace UdemyRestfulAPICourse
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
            // if referencing a connection string set in the appsettings.json file...
            // publish the db after setting this next step. Right click on the DB. Publish DB. Import Profile. Select the file you downloaded from Azure (When Publish Profile is selected in the Portal). 
            // services.AddDbContext<QuotesDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("QuotesDbContext")));
            services.AddDbContext<QuotesDbContext>(options => options.UseSqlServer(@"Server=.; Initial Catalog=QuotesDb;Trusted_Connection=True;"));
            services.AddControllers();
            services.AddMvc().AddXmlDataContractSerializerFormatters();
            services.AddResponseCaching();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-4pi834kh.auth0.com/";
                options.Audience = "https://localhost:44368/";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, QuotesDbContext quotesDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            // quotesDbContext.Database.EnsureCreated();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCaching();

            app.UseAuthentication();

        }
    }
}
