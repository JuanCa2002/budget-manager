using BudgetManager.Mappings;
using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

var policyAuthenticatedUsers = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(policyAuthenticatedUsers));
});
builder.Services.AddTransient<IAccountTypesRepository, AccountTypesRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IExportService, ExportService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<SignInManager<User>>();

// Configure Context Accesor
builder.Services.AddHttpContextAccessor();

// Configure Identity
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;

}).AddErrorDescriber<ErrorMessageIdentity>()
.AddDefaultTokenProviders();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/User/LogIn";
});

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
