using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using GrapeCity.ActiveReports.Aspnetcore.Designer;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;

using System.Text;
using WebDesigner_CustomSharedDataSources.Services;
using WebDesigner_CustomSharedDataSources.Implementation;

namespace WebDesigner_CustomSharedDataSources
{
	public class Startup
	{
		public static readonly DirectoryInfo ResourcesRootDirectory = 
			new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "resources" + Path.DirectorySeparatorChar));
		private static readonly DirectoryInfo TemplatesRootDirectory =
			new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "templates" + Path.DirectorySeparatorChar));

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
				.AddSingleton<ISharedDataSourceService, SharedDataSourceService>()
				.AddSingleton<IResourcesService, ResourceService>()
				.AddSingleton<ITemplatesService>(new FileSystemTemplates(TemplatesRootDirectory))
				.AddSingleton<IDataSetsService>(new CustomDataSetTemplates())
				.AddMvc(options => options.EnableEndpointRouting = false)
				.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, 
			IWebHostEnvironment env, 
			IResourcesService resourcesService, 
			ISharedDataSourceService sharedDataSourceService,
			IDataSetsService dataSetsService)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			var pathToConfig = Path.Combine(Environment.CurrentDirectory, "GrapeCity.ActiveReports.config");
			var sectionResourcesService = resourcesService as ISectionResourcesService;

			app.UseReporting(config =>
			{
				config.UseCustomStore(id =>
				{
					if (".rpx".Equals(Path.GetExtension(id), StringComparison.InvariantCultureIgnoreCase))
						return sectionResourcesService.GetSectionReport(id);
					return resourcesService.GetReport(id);
				});
				config.UseConfig(pathToConfig);
			});

			app.UseDesigner(config =>
			{
				config.UseCustomStore(resourcesService);
				config.UseDataSetTemplates(dataSetsService);
				config.UseSharedDataSources(sharedDataSourceService);
				config.UseConfig(pathToConfig);
			});

			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
