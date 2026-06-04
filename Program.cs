using Backend.Data;
using Backend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var aspnetcoreENV = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (string.IsNullOrEmpty(aspnetcoreENV)) throw new ArgumentException("Not found ASPNETCORE_ENVIRONMENT Variable");
var builder = WebApplication.CreateBuilder(args);
AppSetting.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddSingleton<DbContext>();

// Register Repositories
builder.Services.AddAutoScope();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
//builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
//builder.Services.AddScoped<IQueueRepository, QueueRepository>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IExamRepository, ExamRepository>();
//builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// CORS
var origins = (builder.Configuration["CorsOrigins"] ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsOrigins", policy =>
    {
        if (origins.Length == 1 && origins[0] == "*") policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        else policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key") ?? "YourSuperSecretKeyForJWT1234567890!");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{AppSetting.AssemblyName} ({AppSetting.DateModified}) - {aspnetcoreENV}", Version = AppSetting.AssemblyVersion });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = $@"Standard Authorization header using the Bearer scheme. Example: ""bearer [token]"" 
                        <br>Test Key (actor: Note, role: ADMIN) <br />
                        <br>Please copy text below and paste in value input. <br /> 
                        <textarea readonly style='height:150px;min-height:unset;'>
                            Bearer
                        </textarea>"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", $"{AppSetting.AssemblyName} v1"));
}

app.UseHttpsRedirection();

app.UseCors("CorsOrigins");

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


public static class AppSetting
{
    public static IConfiguration? Configuration { get; set; }
    public static string? DateModified { get; } = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToString("dd/MM/yyyy - HH:mm:ss", new System.Globalization.CultureInfo("en-EN", false));
    public static string? AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;
    public static string? AssemblyVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    public static string? SystemMessage(string? code) => Configuration?[$"SystemMessage:{code?.ToUpper()}"];
}

public static class SetupServiceLifeTime
{
    public static void AddAutoScope(this IServiceCollection services)
    {
        List<Type> allType = new List<Type>();
        List<string> nsRange = new List<string> { $"{AppSetting.AssemblyName}.Repositories" };
        nsRange.ForEach(n =>
        {
            List<Type> srvTyp = Assembly.GetExecutingAssembly().GetTypes()
                                    .Where(t => t.Namespace != null && t.Namespace.StartsWith(n))
                                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic).ToList();

            allType.AddRange(srvTyp);
        });

        if (allType.Any())
        {
            allType.ForEach(clss =>
            {
                var intrf = clss.GetInterfaces().FirstOrDefault();
                if (intrf != null)
                {
                    services.AddScoped(intrf, clss);
                }
                else
                {
                    services.AddScoped(clss);
                }
            });
        }
    }
}