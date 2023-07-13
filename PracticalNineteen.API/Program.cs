using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PracticalNineteen.API.Swagger;
using PracticalNineteen.Data.Contexts;
using PracticalNineteen.Data.Repositories;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Register DB Context
builder.Services.AddDbContextPool<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//JWT Authentication
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(x => { x.Cookie.Name = "token"; })
.AddJwtBearer(jwt =>
{
    var key = builder.Configuration["Jwt:Key"];
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RequireExpirationTime = false,//updated when refresh token is added
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();

//idntity user configurations
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

//Add swagger services
builder.Services.AddSwaggerGen();

//For swagger authorization
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

//Register Automapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

builder.Services.AddCors();

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
