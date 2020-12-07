using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace School.Financial
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
            services.AddControllersWithViews();
            var webConfiguration = Configuration.GetSection(nameof(WebConfiguration)).Get<WebConfiguration>();
            services.AddTransient(x => webConfiguration);
            services.AddTransient(x => new Dac.SchoolFinancialContext(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<Dac.IEducationAreaDac, Dac.Impl.EducationAreaDac>();
            services.AddTransient<Dac.ISchoolDac, Dac.Impl.SchoolDac>();
            services.AddTransient<Dac.IBankAccountDac, Dac.Impl.BankAccountDac>();
            services.AddTransient<Dac.IBudgetDac, Dac.Impl.BudgetDac>();
            services.AddTransient<Dac.IPartnerDac, Dac.Impl.PartnerDac>();
            services.AddTransient<Dac.ITransactionDac, Dac.Impl.TransactionDac>();
            services.AddTransient<Dac.IBringForwardDac, Dac.Impl.BringForwardDac>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
