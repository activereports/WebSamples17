using GrapeCity.ActiveReports.Aspnetcore.Designer;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using WebDesignerAngularCore.Services;
using WebDesignerAngularCore.Implementation;

internal class Program
{
    private static readonly DirectoryInfo ResourcesRootDirectory =
            new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "resources" + Path.DirectorySeparatorChar));

    private static readonly DirectoryInfo TemplatesRootDirectory =
        new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "templates" + Path.DirectorySeparatorChar));

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services
                .AddReporting()
                .AddDesigner()
                .AddSingleton<ITemplatesService>(new FileSystemTemplates(TemplatesRootDirectory))
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseReporting(config => config.UseFileStore(ResourcesRootDirectory));
        app.UseDesigner(config =>
        {
            config.UseFileStore(ResourcesRootDirectory, false);
            config.UseDataSetTemplates(new CustomDataSetTemplates());
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}