﻿using System;
using System.IO;
using System.Net.Http;

using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Aspnetcore.Designer.Services;

using WebDesignerCustomStore.Implementation.Storage;


namespace WebDesignerCustomStore.Implementation.CustomStore
{
	public class CustomStoreResourceLocator : ResourceLocator
	{
		private HttpClient _http;
		private ICustomStorage _db;

		public CustomStoreResourceLocator(ICustomStorage database)
		{
			_db = database;
			_http = new HttpClient();
		}

		public override Resource GetResource(ResourceInfo resourceInfo)
		{
			// Check if there is external resource request
			if (IsURL(resourceInfo.Name))
				return GetExternalResource(resourceInfo.Name);

			// Search non-null resource in all existing database collections
			var resource = _db.GetResource(resourceInfo.Name);
			return resource;
		}

		private Resource GetExternalResource(string uri)
		{
			var request = _http.GetStreamAsync(uri);
			var stream = request.Result;

			return new Resource(stream, null);
		}

		/// <summary>
		/// Creates new <see cref="Stream" /> from supported resource object.
		/// </summary>
		/// <param name="obj">Resource object</param>
		/// <returns><see cref="Stream"/> containing input resource object.</returns>
		/// <remarks>
		/// Supported resource types: 
		/// <list type="bullet">
		///		<item><see cref="Images.ImageInfo"/></item>
		///		<item><see cref="Reports.ReportInfo"/></item>
		///		<item><see cref="Templates.Template"/></item>
		/// </list>
		/// </remarks>
		public static Stream ToStream(object obj)
		{
			var val = obj.GetType().GetProperty("Content")?.GetValue(obj);
			var bytes = val as byte[];

			if (bytes is null)
				return null;

			return new MemoryStream(bytes);
		}

		private static bool IsURL(string uri)
		{
			return Uri.TryCreate(uri, UriKind.Absolute, out var result)
				&& (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
		}

		public static Type GetResourceType(string id)
		{
			var ext = Path.GetExtension(id);

			if (ext.EndsWith(".rdlx-theme"))
				return typeof(Theme);

			if (CustomStoreService.MimeTypeByExtension.ContainsKey(ext))
				return typeof(Images.ImageInfo);

			if (ext == ".rdl" || ext == ".rdlx" || ext == ".rpx")
				return typeof(Reports.ReportInfo);

			throw new NotSupportedException();
		}
	}
}
