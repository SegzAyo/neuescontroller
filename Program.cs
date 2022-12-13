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

builder.Services.AddSwaggerGen(option =>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

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