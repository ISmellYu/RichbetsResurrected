using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RichbetsResurrected.Communication;
using RichbetsResurrected.Communication.Client.Hub;
using RichbetsResurrected.Communication.Crash.Hub;
using RichbetsResurrected.Communication.Roulette.Hub;
using RichbetsResurrected.Identity;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Services;
using RichbetsResurrected.Web;
using Westwind.AspNetCore.LiveReload;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSetting("https_port", "57680");
builder.WebHost.UseSetting("http_port", "57681");

// builder.WebHost.UseUrls("https://*:57680");
builder.WebHost.UseUrls("http://*:57681");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddHttpsRedirection(options => options.HttpsPort = 57680);
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddDirectoryBrowser();

builder.Services.AddAuthStuff(builder.Configuration);

builder.Services.AddLiveReload();
builder.Services.AddSignalR(
    options => 
        options.EnableDetailedErrors = true);

builder.Services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API", Version = "v1"
    });
    c.EnableAnnotations();
    c.AddSignalRSwaggerGen(o => o.ScanAssembly(typeof(RouletteHub).Assembly));
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new DefaultIdentityModule(builder.Configuration));
    containerBuilder.RegisterModule(new DefaultCommunicationModule());
    containerBuilder.RegisterModule(new DefaultServiceModule());
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

var app = builder.Build();

// GlobalHost.DependencyResolver = new AutofacDependencyResolver(app.Services.GetAutofacRoot());

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseShowAllServicesMiddleware();
    app.UseLiveReload();
}
else
{
    // app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
    //app.UseHttpsRedirection();
}

app.UseRouting();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "UnityFiles")),
    RequestPath = "/unity",
    ServeUnknownFileTypes = true
});

app.UseCookiePolicy();

// app.UseStaticFiles(new StaticFileOptions()
// {
//     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
//     ServeUnknownFileTypes = true
//
// });
// app.UseDirectoryBrowser(new DirectoryBrowserOptions()
// {
//     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
// });

app.UseAuthentication();
app.UseAuthorization();



// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
    endpoints.MapHub<RouletteHub>("/rouletteHub");
    endpoints.MapHub<ClientHub>("/clientHub");
    endpoints.MapHub<CrashHub>("crashHub");
});

app.UseStatusCodePagesWithRedirects("/errors/{0}");

// Seed Database
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         //                    context.Database.Migrate();
//         context.Database.EnsureCreated();
//         SeedData.Initialize(services);
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred seeding the DB");
//     }
// }

app.Run();