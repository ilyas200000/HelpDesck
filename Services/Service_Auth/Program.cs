using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Data;
using Service_Auth.Models.Identity;


namespace Service_Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("AuthConnection")
                ?? throw new InvalidOperationException("Connection string 'AuthConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Capture database exceptions resolvable by migration
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Identity services
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyAllowedOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddRazorPages();

            // Add and configure IdentityServer
            builder.Services.AddIdentityServer(options =>
            {
                // General IdentityServer options
                options.Authentication.CoordinateClientLifetimesWithUserSession = true;
                

            })
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                })
                .AddInMemoryApiScopes(new ApiScope[]
                {
                    new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
                })
                .AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "AuthClient",
                        ClientSecrets = { new Secret("Secret1".Sha256()) },
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowedScopes = { "openid", "profile", IdentityServerConstants.LocalApi.ScopeName },
                        AllowOfflineAccess = true,
                        /*AccessTokenLifetime = 3600, // 1 hour
                        AbsoluteRefreshTokenLifetime = 86400, // 1 day
                        SlidingRefreshTokenLifetime = 43200, // 12 hours
                        RefreshTokenUsage = TokenUsage.ReUse,
                        RefreshTokenExpiration = TokenExpiration.Sliding*/
                    }
                })
                .AddAspNetIdentity<ApplicationUser>();

            // Add logging for Duende events at debug level
            builder.Services.AddLogging(options =>
            {
                options.AddFilter("Duende", LogLevel.Debug);
            });

            builder.Services.AddLocalApiAuthentication();
            builder.Services.AddControllers()
                           .AddJsonOptions(options =>
                           {
                               options.JsonSerializerOptions.PropertyNamingPolicy = null;
                           });

            // Extend the userinfo endpoint to include roles
            builder.Services.AddTransient<IProfileService, CustomProfileService>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireDroitParametrageCréationUtilisateurs", policy =>
                   policy.RequireClaim("droit", "Parametrage (création utilisateurs)"));

                options.AddPolicy("RequireDroitParametrageListeEtModificationUtilisateurs", policy =>
                    policy.RequireClaim("droit", "Parametrage (Liste et modification utilisateurs)"));

                options.AddPolicy("RequireDroitParametrageTypeEntite", policy =>
                    policy.RequireClaim("droit", "Parametrage (Type/Entite)"));

            });


            var app = builder.Build();

            app.UseCors("MyAllowedOrigins");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Add authentication and authorization middleware
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });

            app.MapRazorPages();
            app.Run();
        }
    }
}
