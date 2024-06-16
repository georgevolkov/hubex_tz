using Hubex.Module.Adm.Data;
using Hubex.Module.Adm.Services;
using Hubex.Module.Adm.Services.Extensions;
using Hubex.Module.Adm.Services.Interfaces;
using Hubex.Module.Work.Data;
using Hubex.Module.Work.Services;
using Hubex.Module.Work.Services.Interfaees;
using Hubex.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AdmConnection")));

builder.Services.AddDbContext<WorkDbContext> (options =>
   options.UseNpgsql (builder.Configuration.GetConnectionString ("WorkConnection")));

builder.Services.AddControllers ();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer ();
builder.Services.AddSwaggerGen ();

builder.Services.AddScoped<IAdmTaskUserCacheService, AdmTaskUserCacheService> ();
builder.Services.AddScoped<IAdmUserListCategoryService, AdmUserListCategoryService> ();
builder.Services.AddScoped<IWorkTaskUserCacheService, WorkTaskUserCacheService> ();
builder.Services.AddTransient<PredicateExtensions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection ();

app.UseAuthorization ();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers ();

app.Run ();
