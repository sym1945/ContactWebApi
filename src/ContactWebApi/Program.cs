using ContactWebApi.App;
using ContactWebApi.Extensions;
using ContactWebApi.Filters.Actions;
using ContactWebApi.Infra;


var builder = WebApplication.CreateBuilder(args);

var cofiguration = builder.Configuration;

builder.Services.AddControllers(o =>
    {
        o.Filters.Add<ImportDataTypeActionFilter>();
    });

builder.Services.AddScoped<ImportDataTypeActionFilter>();
builder.Services.AddDateOnlyStringConverter();
builder.Services.ConfigureApp(cofiguration);
builder.Services.ConfigureInfra(cofiguration);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
    {
        o.UseDateOnlyStringConverter();
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionMiddleware();

app.MapControllers();

app.Services.InitInfra();

app.Run();
