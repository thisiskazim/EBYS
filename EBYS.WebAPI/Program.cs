using EBYS.Application.Common.Interface;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Application.Mapping;
using EBYS.Application.Services;
using EBYS.Application.Services.GelenEvrakService;
using EBYS.Application.Services.GidenEvrakService;
using EBYS.Application.Services.MuhatapService;
using EBYS.Persistence;
using EBYS.Persistence.Repository;
using EBYS.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder =WebApplication.CreateBuilder(args);

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
});



builder.Services.AddControllers()
    .AddNewtonsoftJson();


builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


var app = builder.Build();


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
