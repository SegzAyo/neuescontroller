using System.Text;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Neues.Core.Interface;
using Neues.Infrastructure;
using Neues.Interface;
using NeuesCore.Data;

var builder = WebApplication.CreateBuilder(args);

//string connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSwaggerGen(option =>{    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme    {        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n",        In = ParameterLocation.Header,        Name = "Authorization",        Type = SecuritySchemeType.Http,        BearerFormat = "JWT",        Scheme = "Bearer"    });    option.AddSecurityRequirement(new OpenApiSecurityRequirement()                {                   {                        new OpenApiSecurityScheme                        {                            Reference = new OpenApiReference                            {                                Type=ReferenceType.SecurityScheme,                                Id="Bearer"                            }                        },                        new string[]{}                    }                });});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    .AddJwtBearer(options =>    {        options.RequireHttpsMetadata = false;        options.SaveToken = true;        options.TokenValidationParameters = new TokenValidationParameters        {            ValidIssuer = "http://yourdomain.com",            ValidAudience = "http://yourdomain.com",            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SOME_RANDOM_KEY_DO_NOT_SHARE")),            ClockSkew = TimeSpan.Zero        };    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<NeuesDbContext>(options => options.UseInMemoryDatabase("WebApiDatabase"));
builder.Services.AddScoped<IPost, PostSqlService>();
builder.Services.AddScoped<IMainComment, MainSqlCommentService>();
builder.Services.AddScoped<ISubComment, SubCommentSqlService>();
builder.Services.AddScoped<IUser, userSqlService>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

/*builder.Services.AddScoped(option => {
    return new BlobServiceClient(builder.Configuration.GetConnectionString("BlobConnectionString"));
    });*/


builder.Services.AddDbContext<NeuesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection"));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors("corsapp");

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();