using System.IO;
using System.Text;
using GrapeCity.ActiveReports.Aspnetcore.Designer;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebDesigner_Custom.Data;
using WebDesigner_Custom.Implementation;
using WebDesigner_Custom.Services;

namespace WebDesigner_Custom;

public class Startup
{
	private static readonly DirectoryInfo TemplatesRootDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "templates" + Path.DirectorySeparatorChar));

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		services
			.AddReporting()
			.AddDesigner()
			.AddDbContext<ReportsDbContext>(ServiceLifetime.Singleton)
			.AddSingleton<ReportService>()
			.AddSingleton<ITemplatesService>(new FileSystemTemplates(TemplatesRootDirectory))
			.AddSingleton<IDataSetsService>(new CustomDataSetTemplates())
			.AddSingleton<IResourcesService>(s => new CustomResourceService(s.GetRequiredService<ReportService>()))		
			.AddMvc(options => options.EnableEndpointRouting = false)
			.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResourcesService resourcesService, IDataSetsService dataSetsService)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		
		app.UseFileServer();
			
		app.UseReporting(config => 
		{
			config.UseCustomStore(resourcesService.GetReport);
		});

		app.UseDesigner(config =>
		{
			config.UseCustomStore(resourcesService);
			config.UseDataSetTemplates(dataSetsService);
		});
        
		app.UseMvc();
	}
}