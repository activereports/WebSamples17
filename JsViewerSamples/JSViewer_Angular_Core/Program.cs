using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using JSViewer_Angular_Core.Controllers;

public class Program
{
    public const string EmbeddedReportsPrefix = "JSViewer_Angular_Core.Reports";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddReporting();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseReporting(settings =>
        {
            settings.UseEmbeddedTemplates(EmbeddedReportsPrefix, typeof(ReportsController).Assembly);
            settings.UseCompression = true;
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}