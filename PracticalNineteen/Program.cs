using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Data.Contexts;
using PracticalNineteen.Data.Repositories;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<UserIdentityModel, IdentityRole>(opt =>
                {
                    opt.SignIn.RequireConfirmedEmail = false;

                    opt.Password.RequiredUniqueChars = 0;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddScoped<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IUserRepository,UsersRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
