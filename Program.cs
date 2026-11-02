
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories;
using MovieTicketBookingSystem.API.Repositories.Interfaces;
using MovieTicketBookingSystem.API.Services;
using MovieTicketBookingSystem.API.Services.Interfaces;
using System.Text;
using MovieTicketBookingSystem.API.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// 1. Add Controllers and OpenAPI
// --------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --------------------------------------------------
// 2. Database Configuration
// --------------------------------------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------------------------------------
// 3. ASP.NET Core Identity Configuration
// --------------------------------------------------

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password requirements
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        // User settings
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// --------------------------------------------------
// 4. JWT Authentication Configuration
// --------------------------------------------------

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT Audience is missing.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero
        };
    });

// --------------------------------------------------
// 5. Movie Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// --------------------------------------------------
// 6. Theatre Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// --------------------------------------------------
// 7. Screen Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

// --------------------------------------------------
// 8. Seat Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

// --------------------------------------------------
// 9. Show Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

// --------------------------------------------------
// 10. Booking Dependency Injection
// --------------------------------------------------

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();


builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IAuthService, AuthService>();



// --------------------------------------------------
// 11. Build Application
// --------------------------------------------------

var app = builder.Build();

// Seed application roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create role: {role}");
            }
        }
    }
}

// Seed development Admin account
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await AdminSeeder.SeedAdminAsync(
        scope.ServiceProvider,
        app.Configuration);
}

// --------------------------------------------------
// 12. Configure HTTP Request Pipeline
// --------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}  

app.UseHttpsRedirection();

// Authentication must execute before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
