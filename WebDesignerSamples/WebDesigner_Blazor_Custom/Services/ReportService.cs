using GrapeCity.ActiveReports.Aspnetcore.Designer.Utilities;
using GrapeCity.ActiveReports.PageReportModel;
using WebDesigner_Blazor_Custom.Data;
using WebDesigner_Blazor_Custom.Implementation;

namespace WebDesigner_Blazor_Custom.Services;

internal class ReportService
{
    private readonly ReportsDbContext _reportsDbContext;
    
    public ReportService(ReportsDbContext reportsDbContext)
    {
        _reportsDbContext = reportsDbContext;
    }

    public Report GetReport(string id)
    {
        var reportInfo = _reportsDbContext.Reports.FirstOrDefault(r => r.Id == id);
        var report = ReportConverter.FromXML(reportInfo.Content);
        report.Site = new ReportSite(new CustomStoreResourceLocator());
        return report;
    }

    public List<ReportInfo> GetReports()
    {
        var reports = _reportsDbContext.Reports.ToList();
        return reports;
    }

    public void SaveReport(string name, Report report, bool temporary=false)
    {
        var reportInfo = _reportsDbContext.Reports.FirstOrDefault(r => r.Id == name);
        if (reportInfo != null)
        {
            if (reportInfo.Readonly)
                throw new Exception("Original report cannot be changed, use 'Save As' with new report name");

            reportInfo.Content = ReportConverter.ToXml(report);
        }
        else
        {
            _reportsDbContext.Reports.Add(new ReportInfo()
            {
                Id = name,
                Name = name,
                Content = ReportConverter.ToXml(report),
                Type = report.IsFixedPageReport ? "FPL" : "CPL",
                Temporary = temporary
            });
        }
        
        _reportsDbContext.SaveChanges();
    }

    public void DeleteReport(string id)
    {
        var reportInfo = _reportsDbContext.Reports.FirstOrDefault(r => r.Id == id);
        _reportsDbContext.Reports.Remove(reportInfo);
        _reportsDbContext.SaveChanges();
    }
}