using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using MVCTask;
using MVCTask.Interfaces;
using MVCTask.Repositories;
using MVCTask.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var policyUsersAuthenticates = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews(
    options => {
        options.Filters.Add(new AuthorizeFilter(policyUsersAuthenticates));
    }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    }).AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDbContext") ?? throw new InvalidOperationException("Connection string 'TaskDbContext' not found.")));

builder.Services.AddAuthentication().AddMicrosoftAccount(options => {
    options.ClientId = builder.Configuration["MicrosoftClientId"];
    options.ClientSecret = builder.Configuration["MicrosoftSecretId"];
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    options => {
        // Configure your application cookie when the user is not authenticated
        options.LoginPath = "/users/Login";
        options.AccessDeniedPath = "/users/login";
    });

builder.Services.AddLocalization(options => {
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.SetDefaultCulture(Constants.CulturesUISupported[0].Value)
        .AddSupportedCultures(Constants.CulturesUISupported.Select(c => c.Value).ToArray())
        .AddSupportedUICultures(Constants.CulturesUISupported.Select(c => c.Value).ToArray());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
