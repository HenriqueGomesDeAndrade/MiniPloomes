using Microsoft.OpenApi.Models;
using MiniPloomes.Entities;
using MiniPloomes.Persistence;
using MiniPloomes.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MiniPloomesContext>();
builder.Services.AddScoped<IMiniPloomesRepository, MiniPloomesRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiniPloomesAPI",
        Description = "Teste Prático 2",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Henrique",
            Email = "hgomes.andrade@gmail.com",
            Url = new Uri("https://github.com/HenriqueGomesDeAndrade")
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "MiniPloomes.xml");
    o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
