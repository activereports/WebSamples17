using System;
using System.IO;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Utilities;
using WebDesigner_ReportParts.Services;

namespace WebDesigner_ReportParts.Implementation
{
	public class FileSystemTemplates : ITemplatesService
	{
		readonly DirectoryInfo _rootFolder;

		public FileSystemTemplates(DirectoryInfo rootFolder)
		{
			_rootFolder = rootFolder;
		}

		public TemplateInfo[] GetTemplatesList()
		{
			return new []
			{
				new TemplateInfo
				{
					Id = "A4 Page Treadstone.rdlx",
					Name = "A4 Page Treadstone"
				},
				new TemplateInfo
				{
					Id = "A4 Landscape RDLX Treadstone.rdlx",
					Name = "A4 Landscape RDLX Treadstone"
				}
			};
		}

		public byte[] GetTemplate(string id)
		{
			var fullPath = Path.Combine(_rootFolder.FullName, id);

			if (!File.Exists(fullPath)) throw new FileNotFoundException();

			var templateXml = File.ReadAllBytes(fullPath);
			var template = ReportConverter.FromXML(templateXml);
			var templateJson = ReportConverter.ToJson(template);
			return templateJson;
		}

		 // Gets a thumbnail with specific name from Embedded Images
		public TemplateThumbnail GetTemplateThumbnail(string id)
		{
			var imagePath = Path.Combine(_rootFolder.FullName, Path.GetFileNameWithoutExtension(id) + ".png");
			return new TemplateThumbnail()
			{
				Data = Convert.ToBase64String(File.ReadAllBytes(imagePath)),
				MIMEType = "image/png"
			};
		}
	}
}
