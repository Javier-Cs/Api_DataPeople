using Api_DataPeople.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//BASE DE DATOS


//AGREGAR SERVICIOS
builder.Services.AddScoped<IPeopleDataService, PeopleDataService>();
builder.Services.AddHttpClient();


//CONFIGURACION DE CORS
builder.Services.AddCors(options =>{
    options.AddPolicy("AllowAstroApp",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4321",
                "https://datospr.cedesystem.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// configuracion de swagger
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Operaciones V1");
    c.RoutePrefix = "swagger";
});



// MODO DE DESARROLLO
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseCors("AllowAstroApp");


// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
