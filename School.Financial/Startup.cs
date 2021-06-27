using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

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
            var connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            connectionString = connectionString
                .Replace("Data Source=", "server=")
                .Replace("127.0.0.1:", "127.0.0.1;port=")
                .Replace("User Id=", "userid=");
            services.AddTransient(x => new Dac.SchoolFinancialContext(connectionString));

            services.AddTransient<Dac.IUserInfoDac, Dac.Impl.UserInfoDac>();
            services.AddTransient<Dac.IEducationAreaDac, Dac.Impl.EducationAreaDac>();
            services.AddTransient<Dac.ISchoolDac, Dac.Impl.SchoolDac>();
            services.AddTransient<Dac.IBankAccountDac, Dac.Impl.BankAccountDac>();
            services.AddTransient<Dac.IBudgetDac, Dac.Impl.BudgetDac>();
            services.AddTransient<Dac.IPartnerDac, Dac.Impl.PartnerDac>();
            services.AddTransient<Dac.ISchoolYearDac, Dac.Impl.SchoolYearDac>();
            services.AddTransient<Dac.ITransactionDac, Dac.Impl.TransactionDac>();
            services.AddTransient<Dac.IIncomeReceiptDac, Dac.Impl.IncomeReceiptDac>();
            services.AddTransient<Dac.IBringForwardDac, Dac.Impl.BringForwardDac>();

            services.AddTransient<Services.IIdentityService, Services.Impl.IdentityService>();

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Index";
                });
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
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
