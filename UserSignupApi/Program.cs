global using Microsoft.EntityFrameworkCore;
using UserSignupApi.Data;
using UserSignupApi.Repository;
using UserSignupApi.Services;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = builder.Environment.IsProduction() ? "ProdConnection" : builder.Environment.IsEnvironment("UAT") ? "UatConnection" : "LocalConnection";

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(dbConnection));
});

builder.Services.BuildServiceProvider().GetService<DataContext>().Database.Migrate();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
