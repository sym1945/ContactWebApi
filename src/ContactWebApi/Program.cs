using ContactWebApi.App;
using ContactWebApi.Infra;

var builder = WebApplication.CreateBuilder(args);

var cofiguration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.ConfigureApp(cofiguration);
builder.Services.ConfigureInfra(cofiguration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
