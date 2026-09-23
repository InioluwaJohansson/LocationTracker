using System.Text;
using LocationTracker.Context;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Implementations.Services;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.RealTime.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICustomMarkerRepo, CustomMarkerRepo>();
builder.Services.AddScoped<ICoordinateRepo, CoordinateRepo>();
builder.Services.AddScoped<IJourneySessionRepo, JourneySessionRepo>();
builder.Services.AddScoped<IRouteLogRepo, RouteLogRepo>();

builder.Services.AddScoped<ICustomMarkerService, CustomMarkerService>();
builder.Services.AddScoped<ICoordinateService, CoordinateService>();
builder.Services.AddScoped<IJourneySessionService, JourneySessionService>();
builder.Services.AddScoped<IRouteLogService, RouteLogService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = null;
    options.Audience = jwtSection["Audience"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/LocationTracker")) context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value.Errors.Select(e => e.ErrorMessage)
            });

        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(errors));

        return new BadRequestObjectResult(errors);
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var appData = builder.Configuration.GetSection("ApplicationDetails");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = appData["AppName"], Version = "v1"});
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 300;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddCors(x => x.AddPolicy(appData["AppName"], c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy(appData["AppName"], policy =>
    {
        policy.WithOrigins(appData["HomeUrl"]).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("LocationTrackerContext");
builder.Services.AddDbContext<LocationTrackerContext>(c => c.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 42))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(appData["AppName"]);
app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<LocationTrackerHub>("/LocationTracker");

app.Use(async (context, next) =>
{
    Console.WriteLine($"➡️ Incoming Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
