using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using Microsoft.EntityFrameworkCore;

namespace WebDesigner_Custom.Data;

public class ReportsDbContext : DbContext
{
    public DbSet<ReportInfo> Reports { get; set; }

    private string _dbPath { get; }

    public ReportsDbContext()
    {
        _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "resources", "Storage.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={_dbPath}");
    }
}

public class ReportInfo : IReportInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Content { get; set; }
    public bool Readonly { get; set; }
}