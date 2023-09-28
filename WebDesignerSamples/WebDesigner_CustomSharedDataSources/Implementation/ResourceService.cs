using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Utilities;
using GrapeCity.ActiveReports.PageReportModel;

namespace WebDesigner_CustomSharedDataSources;

public class ResourceService: IResourcesService, ISectionResourcesService
{
    private readonly Dictionary<string, Report> _tempStorage = new Dictionary<string, Report>();
    private readonly Dictionary<string, SectionReport> _tempSectionStorage = new Dictionary<string, SectionReport>();
    private readonly ResourceLocator _resourceLocator;
    
    public ResourceService()
    {
        _resourceLocator = new DefaultResourceLocator(new Uri(Startup.ResourcesRootDirectory.FullName));
    }
    
    public Report GetReport(string id)
    {
        if (_tempStorage.TryGetValue(id, out var tempReport))
        {
            tempReport.Site = new ReportSite(_resourceLocator);
            return tempReport;
        }
        
        var reportFullPath = Path.Combine(Startup.ResourcesRootDirectory.FullName, id);
        var reportXml = File.ReadAllBytes(reportFullPath);
        var report = ReportConverter.FromXML(reportXml);
        report.Site = new ReportSite(_resourceLocator);
			
        return report;
    }

    public IReportInfo[] GetReportsList()
    {
        return Startup.ResourcesRootDirectory
            .EnumerateFiles("*.rdlx")
            .Select(x =>
            {
                var reportXml = File.ReadAllBytes(x.FullName);
                var report = ReportConverter.FromXML(reportXml);
                return new ReportInfo
                {
                    Id = x.Name, 
                    Name = x.Name,
                    Type = report.IsFixedPageReport ? "FPL" : "CPL"
                };
            }).Cast<IReportInfo>().ToArray();
    }

    public string SaveReport(string name, Report report, bool isTemporary = false)
    {
        if (isTemporary)
        {
            var tempName = Guid.NewGuid() + ".rdlx";
            _tempStorage.Add(tempName, report);
            return tempName;
        }
        
        var reportFullPath = Path.Combine(Startup.ResourcesRootDirectory.FullName, name);
        report.Name = name;
        var reportXml = ReportConverter.ToXml(report);
        File.WriteAllBytes(reportFullPath, reportXml);

        return name;
    }

    public string UpdateReport(string id, Report report)
    {
        return SaveReport(id, report, false);
    }

    public void DeleteReport(string id)
    {
        if (_tempStorage.ContainsKey(id))
        {
            _tempStorage.Remove(id);
            return;
        }
        
        var reportFullPath = Path.Combine(Startup.ResourcesRootDirectory.FullName, id);
        
        if(File.Exists(reportFullPath))
            File.Delete(reportFullPath);
    }

	public SectionReport GetSectionReport(string id)
	{
		if (_tempSectionStorage.TryGetValue(id, out var tempReport))
		{
			return tempReport;
		}

		var reportFullPath = Path.Combine(Startup.ResourcesRootDirectory.FullName, id);

		var sectionReport = new SectionReport();
		using (var reader = new XmlTextReader(File.OpenText(reportFullPath)))
			sectionReport.LoadLayout(reader);

		return sectionReport;
	}

	public string SaveReport(string name, SectionReport report, bool isTemporary = false)
	{
		if (isTemporary)
		{
			var tempName = Guid.NewGuid() + ".rpx";
			_tempSectionStorage.Add(tempName, report);
			return tempName;
		}

		var reportFullPath = Path.Combine(Startup.ResourcesRootDirectory.FullName, name);
		report.Name = name;

		using (var writer = new XmlTextWriter(reportFullPath, Encoding.UTF8))
			report.SaveLayout(writer);

		return name;
	}

	public string UpdateReport(string id, SectionReport report)
	{
		return SaveReport(id, report, false);
	}

	public Uri GetBaseUri()
    {
        return _resourceLocator.BaseUri;
    }

    public Theme GetTheme(string id)
    {
        throw new NotImplementedException();
    }

    public IThemeInfo[] GetThemesList()
    {
        return Array.Empty<IThemeInfo>();
    }

    public byte[] GetImage(string id, out string mimeType)
    {
        throw new NotImplementedException();
    }

    public IImageInfo[] GetImagesList()
    {
        return Array.Empty<IImageInfo>();
    }
}

public class ReportInfo : IReportInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}

class ReportSite : ISite
{
    private readonly ResourceLocator _resourceLocator;

    public ReportSite(ResourceLocator resourceLocator) => 
        _resourceLocator = resourceLocator;

    public object GetService(Type serviceType) =>
        serviceType == typeof(ResourceLocator) ? _resourceLocator : null;

    public IComponent Component => null;
    public IContainer Container => null;
    public bool DesignMode => false;
    public string Name { get; set; }
}



