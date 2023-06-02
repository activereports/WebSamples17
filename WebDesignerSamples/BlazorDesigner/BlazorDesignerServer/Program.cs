using BlazorDesignerServer.Implementation;
using BlazorDesignerServer.Services;
using GrapeCity.ActiveReports.Aspnetcore.Designer;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using Microsoft.AspNetCore.SignalR;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var ResourcesRootDirectory =
	new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "resources"));

var TemplatesRootDirectory =
	new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "templates"));

var DataSetsRootDirectory =
	new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "datasets"));

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
builder.Services.AddReporting();
builder.Services.AddDesigner();
builder.Services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddServerSideBlazor();
builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = 524288000; //500MB
});
builder.Services.AddSingleton<ITemplatesService>(new FileSystemTemplates(TemplatesRootDirectory));
builder.Services.AddSingleton<IDataSetsService>(new FileSystemDataSets(DataSetsRootDirectory));
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// For use as a server for BlazorWebAssembly
app.UseCors(cors => cors.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithExposedHeaders("Content-Disposition"));

app.UseReporting(config => config.UseFileStore(ResourcesRootDirectory));
app.UseDesigner(config => config.UseFileStore(ResourcesRootDirectory, false));

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
