using System;
using System.Linq;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.PageReportModel;

namespace WebDesigner_Custom.Services;

internal class CustomResourceService : IResourcesService
{
    private readonly ReportService _reportService;

    public CustomResourceService(ReportService reportService)
    {
        _reportService = reportService;
    }

    public Report GetReport(string id)
    {
        return _reportService.GetReport(id);
    }

    public IReportInfo[] GetReportsList()
    {
        return _reportService.GetReports().Cast<IReportInfo>().ToArray();
    }

    public string SaveReport(string name, Report report, bool isTemporary = false)
    {
        var reportId = Uri.UnescapeDataString(name);
        report.Name = reportId;
        var reportName = isTemporary ? Guid.NewGuid() + ".rdlx" : reportId;
        _reportService.SaveReport(reportName, report);
        return reportName;
    }

    public string UpdateReport(string id, Report report)
    {
        _reportService.SaveReport(id, report);
        return id;
    }

    public void DeleteReport(string id)
    {
        _reportService.DeleteReport(id);
    }

    public Uri GetBaseUri()
    {
        throw new NotImplementedException();
    }

    public Theme GetTheme(string id)
    {
        //Implement own themes storage.
        //Update GetThemesList to provide the avilable themes to the designer application
        //Update GetTheme to return specific theme from the storage.

        throw new NotImplementedException("Themes storage not implemented.");
    }

    public IThemeInfo[] GetThemesList()
    {
        return Array.Empty<IThemeInfo>();
    }

    public byte[] GetImage(string id, out string mimeType)
    {
        //Implement own images storage.
        //Update GetImagesList to provide the avilable images to the designer application.
        //Update GetImage to return specific image from the storage.

        throw new NotImplementedException("Images storage not implemented.");
    }

    public IImageInfo[] GetImagesList()
    {
        return Array.Empty<IImageInfo>();
    }
}