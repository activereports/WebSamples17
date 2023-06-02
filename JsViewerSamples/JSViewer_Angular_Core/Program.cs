using System.Reflection;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using JSViewer_Angular_Core.Controllers;
using System.Text;

public class Program
{
    private static readonly string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)?.Replace("file:\\", "");
    public static readonly DirectoryInfo ReportsDirectory = new DirectoryInfo(Path.Combine(CurrentDir, "Reports"));

    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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
            settings.UseFileStore(ReportsDirectory);
            settings.UseCompression = true;
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}