using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;
using System.Data;
using System.Reflection;

SqlMapper.AddTypeHandler(new GuidTypeHandler());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MySecureBackend API",
        Version = "v1",
    });
});

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("UnityPolicy", policy =>
    {
        policy.WithOrigins("https://bvos720.github.io")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

//builder.Services.AddTransient<IEnviroment2D, MemoryEnviromentRepository>();
builder.Services.AddTransient<IEnviroment2D, SQLGameObjectRepository>(o => new SQLGameObjectRepository(sqlConnectionString!));
builder.Services.AddTransient<IObject2D, SQLobject2D>(o => new SQLobject2D(sqlConnectionString!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MySecureBackend API v1");
        options.RoutePrefix = "swagger";
        options.CacheLifetime = TimeSpan.Zero;

        if (!sqlConnectionStringFound)
            options.HeadContent = "<h1 align=\"center\">❌ SqlConnectionString not found ❌</h1>";
    });
}
else
{
    var buildTimeStamp = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
    string currentHealthMessage = $"The API is up 🚀 | Connection string found: {(sqlConnectionStringFound ? "✅" : "❌")} | Build timestamp: {buildTimeStamp}";

    app.MapGet("/", () => currentHealthMessage);
}

app.UseHttpsRedirection();

app.UseCors("UnityPolicy");

app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>().WithTags("Account");

app.MapControllers().RequireAuthorization();

app.Run();
