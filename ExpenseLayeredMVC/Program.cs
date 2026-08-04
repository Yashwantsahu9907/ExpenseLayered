using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add MVC Services
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// HttpClient & API Services
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddHttpClient<CategoryApiService>();
builder.Services.AddHttpClient<ExpenseApiService>();
builder.Services.AddHttpClient<IncomeApiService>();

builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<CategoryApiService>();
builder.Services.AddScoped<ExpenseApiService>();
builder.Services.AddScoped<IncomeApiService>();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();