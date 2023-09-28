﻿using BlazorDesignerServer.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BlazorDesignerServer.Implementation
{
    internal class FileSystemDataSets : IDataSetsService
    {
        private readonly DirectoryInfo _rootFolder;

        public FileSystemDataSets(DirectoryInfo rootFolder)
        {
            _rootFolder = rootFolder;
        }

        private static string DataSetExtension = ".json";

        public object[] GetDataSetsList()
        {
            var dataSetsList = _rootFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                .Select(fileInfo => fileInfo.Name)
                .Where(x => string.Equals(Path.GetExtension(x), DataSetExtension, StringComparison.InvariantCultureIgnoreCase))
                .Select(name => new
                {
                    Id = name,
                    Name = name.Substring(0, name.Length - DataSetExtension.Length),
                })
                .ToArray();
            return dataSetsList;
        }

        public object GetDataSet(string id)
        {
            var name = Uri.UnescapeDataString(id);
            var fullPath = Path.Combine(_rootFolder.FullName, name);

            if (!File.Exists(fullPath)) throw new ArgumentException();

            using (var streamReader = File.OpenText(fullPath))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
