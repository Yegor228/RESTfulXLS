using Microsoft.EntityFrameworkCore;
using RESTfulXLS.Contexts;

namespace RESTfulXLS
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
            services.AddMvc();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Tender}/{action=Index}");
                endpoints.MapControllerRoute(name: "user", pattern: "User/{action=Create}", defaults: new { controller = "User" });
                endpoints.MapControllerRoute(name: "tender", pattern: "Tender/{action=RestfulTenderView}", defaults: new { controller = "Tender" });
            });
        }
    }
}
