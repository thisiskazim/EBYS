using EBYS.Application.Interface;
using EBYS.Persistence;
using EBYS.Persistence.Repository;
using Microsoft.EntityFrameworkCore;



var builder =WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EBYSContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEvrakRepository, EvrakRepository>();
//builder.Services.AddScoped<IMuhatapRepository, MuhatapRepository>();




builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
