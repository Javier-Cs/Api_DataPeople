using Api_DataPeople.Data;
using Api_DataPeople.Repository;
using Api_DataPeople.Services;
using Api_DataPeople.Validacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
//---------------
builder.Services.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(key),

            ClockSkew = TimeSpan.Zero
        };

    options.Events = new JwtBearerEvents {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["access_token"];
            if(!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});


//AGREGAR SERVICIOS
builder.Services.AddScoped<IPeopleDataService, PeopleDataService>();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<UserAuthRepo>();
builder.Services.AddScoped<ContarIntentosRepo>();
builder.Services.AddHttpClient();
builder.Services.Configure<ForwardedHeadersOptions>(
    options => {
        options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    }
);




//BASE DE DATOS
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();


// rate limit de middleware
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        "login",
        config => {
            config.PermitLimit = 21;
            config.Window = TimeSpan.FromMinutes(1);

            config.QueueLimit = 0;
        }
    );
});


//CONFIGURACION DE CORS
builder.Services.AddCors(options =>{
    options.AddPolicy("AllowAstroApp",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4321",
                "https://datospr.cedesystem.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
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
app.UseMiddleware<CsrfMiddleware>();


// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

//app.UseHttpsRedirection();


//user limit
app.UseRateLimiter();

// configuracion de ip
app.UseForwardedHeaders();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
