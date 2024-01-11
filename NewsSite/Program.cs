using Common.UploudImage;
using Data.DbContext;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Service.Services.Advertise;
using Service.Services.Comment;
using Service.Services.News;
using Service.Services.Poll;
using Service.Services.Role;
using Service.Services.SiteVisit;
using Service.Services.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



#region DataBase

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion


#region Identity

builder.Services.AddIdentity<ApplicationUsers, ApplicationRoles>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false; 
})
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

#endregion


#region AddScoped

builder.Services.ConfigureApplicationCookie(options=>options.LoginPath = "/Home");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUploadFiles, UploadFiles>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAdvertiseService, AdvertiseService>();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IVisitService, VisitService>();

#endregion

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

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
   name: "Area",
   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
 );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
