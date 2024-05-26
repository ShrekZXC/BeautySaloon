using BeautySaloon.BL.Auth;
using BeautySaloon.BL.General;
using BeautySaloon.DAL;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BeautySaloonDbContext>(op =>
{
    op.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IDbSession, DbSession>();
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.AddTransient<IServiceService, ServiceService>();
builder.Services.AddTransient<IUserTokenService, UserTokenService>();
builder.Services.AddTransient<IUserSerivce, UserService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IPromotionService, PromotionService>();
builder.Services.AddScoped<IWebCookie, WebCookie>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddSingleton<IEncrypt, Encrypt>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();