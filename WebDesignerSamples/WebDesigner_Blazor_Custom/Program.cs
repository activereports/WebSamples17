using GrapeCity.ActiveReports.Aspnetcore.Designer;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using Microsoft.AspNetCore.SignalR;
using WebDesigner_Blazor_Custom.Data;
using WebDesigner_Blazor_Custom.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ReportsDbContext>(ServiceLifetime.Singleton);
builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<IResourcesService>(s => new CustomResourceService(s.GetRequiredService<ReportService>()));
builder.Services.AddReporting();
builder.Services.AddDesigner();
builder.Services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddServerSideBlazor();
builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = 524288000; //500MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var resourcesService = app.Services.GetRequiredService<IResourcesService>();

app.UseReporting(config => config.UseCustomStore(resourcesService.GetReport));
app.UseDesigner(config => config.UseCustomStore(resourcesService));

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
