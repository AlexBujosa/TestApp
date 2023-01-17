using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using EndPointProject.Service;
using EndPointProject.DB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<JwtManager>();
builder.Services.AddTransient<MongoDb>();
builder.Services.AddCors();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<IUserService, UserService>();
// Accessing to the appsettings.json

#region Authentication
var pemFile = "private_key.pem";
var pem = File.ReadAllText(pemFile);
var rsa = new RSACryptoServiceProvider();
rsa.ImportFromEncryptedPem(pem, builder.Configuration["Key"]);

var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Authority"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = signingCredentials.Key
        };
    }
);
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.Run();
