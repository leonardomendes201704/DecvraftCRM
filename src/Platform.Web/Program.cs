using Microsoft.AspNetCore.Authentication.Cookies;
using Platform.Web.Clients;
using Platform.Web.Constants;
using Platform.Web.Security;
using Platform.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = WebAuthenticationDefaults.CookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.LoginPath = AccountRouteNames.LoginPage;
        options.LogoutPath = AccountRouteNames.LogoutPage;
        options.SlidingExpiration = false;
    });
builder.Services.AddScoped<IWebUserContext, WebUserContext>();
builder.Services.AddScoped<PermissionViewPolicy>();
builder.Services.AddScoped<PlatformApiClient>();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<CrmApiClient>();
builder.Services.AddScoped<FinanceApiClient>();
builder.Services.AddScoped<OrganizationApiClient>();
builder.Services.AddScoped<AuthWebService>();
builder.Services.AddScoped<CrmWebService>();
builder.Services.AddScoped<FinanceWebService>();
builder.Services.AddScoped<OrganizationWebService>();
builder.Services.AddScoped<DashboardWebService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
