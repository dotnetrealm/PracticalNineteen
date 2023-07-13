var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("api", (client) =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API_URL"));
});

builder.Services.AddAuthentication("AccountCookie").AddCookie("AccountCookie", opt =>
{
    opt.Cookie.Name = "AccountCookie";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Account/PageNotFound");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
