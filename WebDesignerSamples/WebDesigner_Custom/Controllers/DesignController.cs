using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebDesigner_Custom.Data;
using WebDesigner_Custom.Services;

namespace WebDesigner_Custom.Controllers
{
	[Route("/")]
	public class DesignController : Controller
	{
		private ReportService _reportService;
		public DesignController(ReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("create");
		}

		[HttpGet("create")]
		public ActionResult Create()
		{
			return View("Index");
		}
		
		[HttpGet("reports")]
		public List<ReportInfo> Reports()
		{
			return _reportService.GetReports();
		}
	}
}
