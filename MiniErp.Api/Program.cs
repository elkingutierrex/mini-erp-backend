using MiniErp.Infrastructure.Persistence;
using MiniErp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// CORS
// =======================

const string CorsPolicyName = "MiniErpCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200", // Angular
                "http://localhost:5173"  // Vite / React
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =======================
// Controllers
// =======================

builder.Services.AddControllers();

// =======================
// Swagger
// =======================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// DB
// =======================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("MiniErpDb");
});

// =======================
// JWT
// =======================

builder.Services.AddScoped<JwtTokenService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// =======================
// App
// =======================

var app = builder.Build();

// =======================
// Middleware
// =======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⬅️ CORS primero
app.UseCors(CorsPolicyName);

// ⬅️ Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =======================
// Seed DB
// =======================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(context);
}

app.Run();
