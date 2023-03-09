using ComputerExam;
using ComputerExam.Common;
using ComputerExam.Common.Logger;
using ComputerExam.DBService;
using ComputerExam.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sentry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowCredentialsPolicy",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["LinkToFrontEnd"])
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddDbContext<DSUContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDekanat"), providerOptions => providerOptions.EnableRetryOnFailure()));
//builder.Services.AddDbContext<BASEPERSONMDFContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("BasePerson"), providerOptions => providerOptions.EnableRetryOnFailure()));
//builder.Services.AddDbContext<ApplicationContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("EOR"), providerOptions => providerOptions.EnableRetryOnFailure()));

//builder.Services.AddIdentity<EorDSU.Models.User, IdentityRole>(
//               opts =>
//               {
//                   opts.Password.RequiredLength = 2;
//                   opts.Password.RequireNonAlphanumeric = false;
//                   opts.Password.RequireLowercase = false;
//                   opts.Password.RequireUppercase = false;
//                   opts.Password.RequireDigit = false;
//               })
//               .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;

    options.LoginPath = "/Account/Login";
    options.SlidingExpiration = true;
});

builder.WebHost.ConfigureServices(configure => SentrySdk.Init(o =>
{
    // Tells which project in Sentry to send events to:
    o.Dsn = builder.Configuration["SentyDSN"]; ;
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
    // Enable Global Mode if running in a client app
    o.IsGlobalModeEnabled = true;
}));

builder.Services.AddServiceCollection();
builder.Services.AddAuthorization();

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["FileLoggerFolder"]));

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Models.User>>();
//    var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    if (userManager.Users.ToList().Count == 0)
//    {
//        List<LoginViewModel> employees = new()
//        {
//            new LoginViewModel
//            {
//                Login = builder.Configuration["AdminLogin"],
//                Password = builder.Configuration["AdminPassword"]
//            },
//            new LoginViewModel
//            {
//                Login = builder.Configuration["UMULogin"],
//                Password = builder.Configuration["UMUPassword"],
//            }
//        };
//        await RoleInitializer.InitializeAsync(employees, userManager, rolesManager);
//    }
//}

app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowCredentialsPolicy");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();