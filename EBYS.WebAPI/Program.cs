using EBYS.Application.Common.Interface;
using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using EBYS.Application.Interfaces.IService.IResmiYaziService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Application.Mapping;
using EBYS.Application.Services;
using EBYS.Application.Services.GelenEvrakService;
using EBYS.Application.Services.GidenEvrakService;
using EBYS.Application.Services.MuhatapService;
using EBYS.Persistence;
using EBYS.Persistence.Gemini;
using EBYS.Persistence.Gemini.Instructions;
using EBYS.Persistence.Gemini.Options;
using EBYS.Persistence.Repository;
using EBYS.Persistence.Services;
using EBYS.WebAPI.Helpers;
using EBYS.WebAPI.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.PostgreSQL;
using System.Reflection;
using System.Text;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
EnvLoader.Load(Directory.GetCurrentDirectory());
var builder = WebApplication.CreateBuilder(args);



var connections = builder.Configuration.GetConnectionString("DbConnection");
// Log tablosundaki kolon ùablonunu belirliyoruz (Hata mesajù, StackTrace vb.)
var columnOptions = new Dictionary<string, ColumnWriterBase>
{
    { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
    { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
    { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
    { "timestamp", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
    { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
    { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails() // Hatanùn tùm detaylarùnù (inner exception vb.) yakalar
    .WriteTo.PostgreSQL(
        connectionString: connections,
        tableName: "logs", // Veritabanùnda otomatik aùùlacak tablo adù
        columnOptions: columnOptions,
        needAutoCreateTable: true) // Tablo yoksa otomatik oluùtur!
    .CreateLogger();

// .NET'in kendi loglama mekanizmasùnù Serilogq'a baùlùyoruz
builder.Host.UseSerilog();



builder.Services
	.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddKendo();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddDbContext<EBYSContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGidenEvrakRepository, GidenEvrakRepository>();
builder.Services.AddScoped<IGidenEvrakService, GidenEvrakService>();

builder.Services.AddScoped<IGidenEvrakAkisService, GidenEvrakAkisService>();
builder.Services.AddScoped<IEimzaService, MockEimzaService>();


builder.Services.AddScoped<IKonuKoduService, KonuKoduService>();


builder.Services.AddScoped<IImzaRotaRepository, ImzaRotaRepository>();
builder.Services.AddScoped<IImzaRotaService, ImzaRotaService>();

builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();

builder.Services.AddScoped<IMuhatapRepository, MuhatapRepository>();
builder.Services.AddScoped<IMuhatapKurumService,KurumService>();

builder.Services.AddScoped<IMuhatapTuzelKisiService, TuzelKisiService>();



builder.Services.AddScoped<IGelenEvrakService, GelenEvrakService>();
builder.Services.AddScoped<IGelenEvrakRepository, GelenEvrakRepository>();

builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("GeminiSettings"));
builder.Services.AddSingleton<ResmiYaziSystemInstructionFactory>();
builder.Services.AddSingleton<IResmiYaziSystemInstructionStrategy, DilekceSystemInstructionStrategy>();
builder.Services.AddSingleton<IResmiYaziSystemInstructionStrategy, UstYaziSystemInstructionStrategy>();
builder.Services.AddSingleton<IResmiYaziSystemInstructionStrategy, IcYazismaSystemInstructionStrategy>();
builder.Services.AddHttpClient<IResmiYaziGeneratorService, GeminiResmiYaziService>()
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(90));


builder.Services.AddValidatorsFromAssemblies(new[] { Assembly.Load("EBYS.Application") });

// 1. Handler'ù servislere kaydet
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// API davranùù ayarlarùnù yapùlandùrùyoruz
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // .NET'in kendi kafasùna gùre otomatik 400 (ProblemDetails) dùnmesini engeller
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddKendo();

builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers(options =>
{
 
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
    options.Filters.Add<ValidationFilter>();
});



builder.Services.AddControllers()
    .AddNewtonsoftJson();


builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();  
app.MapControllers();

app.Run();
