﻿using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace JSViewer_MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return Resource("index.html");
        }

        [HttpGet]
        [Route("{file}")]
        public ActionResult Resource(string file)
        {
            var stream = GetType().Assembly.GetManifestResourceStream("JSViewer_MVC.wwwroot." + file);
            if (stream == null)
                return new HttpNotFoundResult();

            if (Path.GetExtension(file) == ".html")
                return new ContentResult {Content = new StreamReader(stream).ReadToEnd(), ContentType = "text/html"};

            if (Path.GetExtension(file) == ".ico")
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return new FileContentResult(memoryStream.ToArray(), "image/x-icon") {FileDownloadName = file};
                }

            using (var streamReader = new StreamReader(stream))
                return new FileContentResult(System.Text.Encoding.UTF8.GetBytes(streamReader.ReadToEnd()),
                    GetMimeType(file)) {FileDownloadName = file};
        }

        [HttpGet]
        [Route("reports")]
        public ActionResult Reports()
        {
            string[] validExtensions = {".rdl", ".rdlx", ".rdlx-master", ".rpx"};
            var reportsList = GetEmbeddedReports(validExtensions);
            return new JsonResult
            {
                Data = reportsList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// Gets the MIME type from the file extension
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>MIME type</returns>
        private static string GetMimeType(string fileName)
        {
            if (fileName.EndsWith(".css"))
                return "text/css";

            if (fileName.EndsWith(".js"))
                return "text/javascript";

            return "text/html";
        }

        /// <summary>
        /// Gets report names from assembly resources
        /// </summary>
        /// <returns>Report names</returns>
        private static string[] GetEmbeddedReports(string[] validExtensions)
        {
            return typeof(HomeController).Assembly.GetManifestResourceNames()
                .Where(x => x.StartsWith(Startup.EmbeddedReportsPrefix))
                .Where(x => validExtensions.Any(x.EndsWith))
                .Select(x => x.Substring(Startup.EmbeddedReportsPrefix.Length + 1))
                .ToArray();
        }
    }
}