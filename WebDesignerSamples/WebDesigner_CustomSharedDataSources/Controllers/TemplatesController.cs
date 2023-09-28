using Microsoft.AspNetCore.Mvc;
using System;
using WebDesigner_CustomSharedDataSources.Services;

namespace WebDesigner_CustomSharedDataSources.Controllers
{
	[Route("api/templates")]
	[Controller]
	public class TemplatesController : Controller
	{
		[HttpGet("{id}/content")]
		public IActionResult GetTemplate([FromServices] ITemplatesService templatesService, [FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return BadRequest();
			var template = templatesService.GetTemplate(id);

			return File(template, "application/json");
		}

		[HttpGet("{id}/thumbnail")]
		[ResponseCache(Duration = 3600)]
		public IActionResult GetTemplateThumbnail([FromServices] ITemplatesService templatesService, [FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return BadRequest();
			try
			{
				var thumbnail = templatesService.GetTemplateThumbnail(id);
				return Json(thumbnail);
			}
			catch (Exception)
			{
				return NoContent();
			}
		}

		[HttpGet("list")]
		public IActionResult GetTemplatesList([FromServices] ITemplatesService templatesService)
		{
			var templatesList = templatesService.GetTemplatesList();
			return Json(templatesList);
		}
	}
}
