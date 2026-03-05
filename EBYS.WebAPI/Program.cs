using EBYS.Application.Common.Interface;
using EBYS.Application.Interface;
using EBYS.Application.Mapping;
using EBYS.Persistence;
using EBYS.Persistence.Repository;
using EBYS.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder =WebApplication.CreateBuilder(args);

// Add framework services.
builder.Services
	.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add Kendo UI services to the services container
builder.Services.AddKendo();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddDbContext<EBYSContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEvrakRepository, EvrakRepository>();
builder.Services.AddScoped<IMuhatapRepository, MuhatapRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();




builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddKendo();

builder.Services.AddSwaggerGen(c =>
{
    // Kendo/Telerik sýnýflarýnýn Swagger dokümantasyonunda hata çýkarmasýný engeller
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

builder.Services.AddAuthorization();




builder.Services.AddControllers()
    .AddNewtonsoftJson();
//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication(); // 1. Kimsin? (Bilet kontrolü)
app.UseAuthorization();  // 2. Yetkin var mý? (Giriþ izni)
app.MapControllers();

app.Run();
