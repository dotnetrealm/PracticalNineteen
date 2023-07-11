using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PracticalNineteen.Data.Contexts;
using PracticalNineteen.Data.Repositories;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<UserIdentity, IdentityRole>(opt =>
                    {
                        opt.SignIn.RequireConfirmedEmail = false;
                        opt.Password.RequiredUniqueChars = 0;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequireDigit = false;
                        opt.Password.RequireLowercase = false;
                        opt.Password.RequireUppercase = false;
                    })
                    .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = builder.Configuration["Jwt:Key"];
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RequireExpirationTime = false,//updated when refresh token is added
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };

    //after successfull authentication, then add token inside header
    jwt.SaveToken = true;
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContextPool<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine(context);
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
