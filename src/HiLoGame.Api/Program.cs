using HiLoGame.Api.Extensions;
using HiLoGame.Application.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterDependencies();
builder.Services.ConfigureMapping();
builder.Services.AddContext(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UserAgentFilterAttribute>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
