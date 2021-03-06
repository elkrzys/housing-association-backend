using System;
using System.IO;
using System.Text;
using HousingAssociation.Settings;
using HousingAssociation.DataAccess;
using HousingAssociation.ExceptionHandling;
using HousingAssociation.Services;
using HousingAssociation.Utils.Jwt.JwtUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace HousingAssociation
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            
            string logsPath = $"{env.ContentRootPath}/logs";
            if (!Directory.Exists(logsPath)) {
                Directory.CreateDirectory(logsPath);
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger= new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
            Log.Information("Application is starting.");
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddCors();
            services.AddMvc().AddNewtonsoftJson();
            
            services.AddSwaggerGen();
            
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            
            var jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
            services.AddSingleton(jwtConfig);
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddScoped<IJwtUtils, JwtUtils>();


            // Database configuration
            var dbSettings = Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dbSettings.ConnectionString)
                    .UseSnakeCaseNamingConvention()
                    .UseEnumCheckConstraints()
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Services
            services.AddScoped<AddressesService>();
            services.AddScoped<BuildingsService>();
            services.AddScoped<LocalsService>();
            services.AddScoped<UsersService>();
            services.AddScoped<DocumentsService>();
            services.AddScoped<AnnouncementsService>();
            services.AddScoped<IssuesService>();
            services.AddScoped<AuthenticationService>();
            services.AddSingleton<EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Housing Association v1"));
                }
            }

            string documentsPath = $"{env.ContentRootPath}/wwwroot/documents";
           
            if (!Directory.Exists(documentsPath)) {
                Directory.CreateDirectory(documentsPath);
            }
           
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "documents")),
                RequestPath = "/documents"
            });

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}