using MongoDBTraining.Persistence;
using MongoDBTraining.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.ConfigureSevices();
builder.Services.ConfigurePersistence(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB Training API");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
