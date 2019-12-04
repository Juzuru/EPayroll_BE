using EPayroll_BE.Data;
using EPayroll_BE.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors.Security;
using NSwag;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using EPayroll_BE.Utilities;
using EPayroll_BE.Services;

namespace EPayroll_BE
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
            // ===== Add DBContext =====
            services.AddDbContext<EPayrollContext>(options =>
                options.UseSqlServer(@"Data Source=45.119.83.107;Initial Catalog=EPayroll;
                    persist security info=True;Integrated Security=False;TrustServerCertificate=False;
                    uid=sa;password=sa@123456;Trusted_Connection=False;MultipleActiveResultSets=true;"));

            #region Add JWT Authenciation
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = JWTUtilities.tokenValidationParameters;
                });
            #endregion
            
            #region Add Repositories
            services.AddScoped<IFormularRepository, FormularRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPayItemRepository, PayItemRepository>();
            services.AddScoped<IPayPeriodRepository, PayPeriodRepository>();
            services.AddScoped<IPaySlipRepository, PaySlipRepository>();
            services.AddScoped<IPayTypeAmountRepository, PayTypeAmountRepository>();
            services.AddScoped<IPayTypeCategoryRepository, PayTypeCategoryRepository>();
            services.AddScoped<IPayTypeRepository, PayTypeRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ISalaryLevelRepository, SalaryLevelRepository>();
            services.AddScoped<ISalaryModeRepository, SalaryModeRepository>();
            services.AddScoped<ISalaryShiftRepository, SalaryShiftRepository>();
            services.AddScoped<ISalaryTablePositionRepository, SalaryTablePositionRepository>();
            services.AddScoped<ISalaryTableRepository, SalaryTableRepository>();
            #endregion

            #region Add Services
            services.AddScoped<IFormularService, FormularService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPayItemService, PayItemService>();
            services.AddScoped<IPayPeriodService, PayPeriodService>();
            services.AddScoped<IPaySlipService, PaySlipService>();
            services.AddScoped<IPayTypeAmountService, PayTypeAmountService>();
            services.AddScoped<IPayTypeCategoryService, PayTypeCategoryService>();
            services.AddScoped<IPayTypeService, PayTypeService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ISalaryLevelService, SalaryLevelService>();
            services.AddScoped<ISalaryModeService, SalaryModeService>();
            services.AddScoped<ISalaryShiftService, SalaryShiftService>();
            services.AddScoped<ISalaryTablePositionService, SalaryTablePositionService>();
            services.AddScoped<ISalaryTableService, SalaryTableService>();
            #endregion

            services.AddSwaggerDocument(c =>
            {
                c.DocumentName = "EPayroll-Api-Docs";
                c.Title = "EPayroll-API";
                c.Version = "v3";
                c.Description = "The EPayroll API documentation description.";
                c.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Copy 'Bearer ' + valid JWT token into field",
                    In = OpenApiSecurityApiKeyLocation.Header
                }));
                c.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            app.UseCors(builder =>
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            app.UseCookiePolicy();

            app.UseOpenApi(_ => _.DocumentName = "EPayroll-Api-Docs");
            app.UseSwaggerUi3();
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
