using MaseterDEtailsCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
    options.SerializerSettings.PreserveReferencesHandling= Newtonsoft.Json.PreserveReferencesHandling.Objects;
});
builder.Services.AddDbContext<AppDBContext>(option => option.UseSqlServer
(builder.Configuration.GetConnectionString("con")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app= builder.Build();   
app.UseStaticFiles();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllerRoute
    (
    name:"default",
    pattern: "{ controller}/{ action}/{id?}"
    );
app.UseCors(x => {
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();

});

app.Run();
