using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.PageReportModel;
using GrapeCity.ActiveReports.Rendering.Tools;

namespace WebDesigner_CustomSharedDataSources;

public class SharedDataSourceService : ISharedDataSourceService
{
    public SharedDataSourceInfo[] GetSharedDataSourceList()
    {
        return Startup.ResourcesRootDirectory
            .EnumerateFiles("*.rdsx")
            .Select(x =>
            {
                var dataSource = GetDataSource(x.Name);
                return new SharedDataSourceInfo()
                {
                    Name = x.Name, 
                    Type = dataSource.ConnectionProperties.DataProvider
                };
            }).ToArray();
    }

    public DataSource GetDataSource(string name)
    {
        return DataSourceTools.LoadSharedDataSource(Path.Combine(Startup.ResourcesRootDirectory.FullName, name));
    }
}