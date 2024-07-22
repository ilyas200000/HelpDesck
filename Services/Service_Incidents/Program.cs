using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service_Incidents.Data;

namespace Service_Incidents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Récupère la chaîne de connexion à la base dans les paramètres
            string? connect = builder.Configuration.GetConnectionString("IncidentConnect");

            // Add services to the container.
            builder.Services.AddDbContext<IncidentsDbContext>(opt => opt.UseSqlServer(connect));

            builder.Services.AddControllers()
                           .AddJsonOptions(options =>
                           {
                               options.JsonSerializerOptions.PropertyNamingPolicy = null;
                           });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enregistre les contrôleurs
            // No need to add again: builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyAllowedOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    });
            });

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7270";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireDroitDeclaration", policy =>
                    policy.RequireClaim("droit", "Declaration"));

                options.AddPolicy("RequireDroitConsIncidentsDeclarés", policy =>
                    policy.RequireClaim("droit", "Cons Incidents Declarés"));

                options.AddPolicy("RequireDroitConsTousLesIncidents", policy =>
                    policy.RequireClaim("droit", "Cons tous les incidents"));

                options.AddPolicy("RequireDroitAnnulerIncident", policy =>
                    policy.RequireClaim("droit", "Annuler Incident"));

                options.AddPolicy("RequireDroitAssistanceActivationApplications", policy =>
                    policy.RequireClaim("droit", "Assistance Activation d'applications"));

                options.AddPolicy("RequireDroitAssistancePreuveDePaiement", policy =>
                    policy.RequireClaim("droit", "Assistance Preuve de paiement"));

                options.AddPolicy("RequireDroitTraitement", policy =>
                    policy.RequireClaim("droit", "Traitement"));

                options.AddPolicy("RequireDroitParametrageMailingList", policy =>
                    policy.RequireClaim("droit", "Parametrage (Mailing list)"));

                options.AddPolicy("RequireDroitSuivi", policy =>
                    policy.RequireClaim("droit", "Suivi"));

                options.AddPolicy("RequireDroitRapports", policy =>
                    policy.RequireClaim("droit", "Rapports"));

                options.AddPolicy("RequireDroitParametrageCréationUtilisateurs", policy =>
                    policy.RequireClaim("droit", "Parametrage (création utilisateurs)"));

                options.AddPolicy("RequireDroitParametrageListeEtModificationUtilisateurs", policy =>
                    policy.RequireClaim("droit", "Parametrage (Liste et modification utilisateurs)"));

                options.AddPolicy("RequireDroitParametrageTypeEntite", policy =>
                    policy.RequireClaim("droit", "Parametrage (Type/Entite)"));

                options.AddPolicy("RequireDroitTraitementIncidentExterne", policy =>
                    policy.RequireClaim("droit", "Traitement Incident externe"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("MyAllowedOrigins");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
