using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.JeuxDeDonnees;
using UniversiteDomain.JeuxDeDonnees;
using UniversiteDomain.UseCases;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Entities;
using UniversiteEFDataProvider.RepositoryFactories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging setup
builder.Services.AddLogging(options =>
{
    options.ClearProviders();
    options.AddConsole();
});

// Database setup
string connectionString = builder.Configuration.GetConnectionString("MySqlConnection") 
                          ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
builder.Services.AddDbContext<UniversiteDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();



// Identity setup
builder.Services.AddIdentity<UniversiteUser, UniversiteRole>()
    .AddEntityFrameworkStores<UniversiteDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication setup
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VotreCléSecrèteTrèsLongueEtSûre")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<UserManager<UniversiteUser>>();
builder.Services.AddScoped<RoleManager<UniversiteRole>>();

builder.Services.AddScoped<GenerateCsvForUeNotesUseCase>();
builder.Services.AddScoped<UploadCsvForUeNotesUseCase>();
builder.Services.AddScoped<ValidationUseCase>();




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Custom login endpoint with role added to JWT
app.MapPost("/custom-login", async ([FromBody] LoginDto loginDto, UserManager<UniversiteUser> userManager, RoleManager<UniversiteRole> roleManager) =>
{
    var user = await userManager.FindByEmailAsync(loginDto.Email);
    if (user != null && await userManager.CheckPasswordAsync(user, loginDto.Password))
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add user roles to the token
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VotreCléSecrèteTrèsLongueEtSûre"));

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
    return Results.Unauthorized();
});

using(var scope = app.Services.CreateScope())
{
    // On récupère le logger pour afficher des messages. On l'a mis dans les services de l'application
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<UniversiteDbContext>>();
    // On récupère le contexte de la base de données qui est stocké sans les services
    DbContext context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();
    logger.LogInformation("Initialisation de la base de données");
    // Suppression de la BD
    logger.LogInformation("Suppression de la BD si elle existe");
    await context.Database.EnsureDeletedAsync();
    // Recréation des tables vides
    logger.LogInformation("Création de la BD et des tables à partir des entities");
    await context.Database.EnsureCreatedAsync();
}
// Initisation de la base de données
ILogger Logger = app.Services.GetRequiredService<ILogger<BdBuilder>>();
Logger.LogInformation("Chargement des données de test");
using(var scope = app.Services.CreateScope())
{
    UniversiteDbContext context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();
    IRepositoryFactory repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();   
    // C'est ici que vous changez le jeu de données pour démarrer sur une base vide par exemple
    BdBuilder seedBD = new BasicBdBuilder(repositoryFactory);
    await seedBD.BuildUniversiteBdAsync();
}

app.Run();

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}