using System;
using System.IO;
using GrapeCity.ActiveReports;

namespace WebDesigner_Custom.Implementation;

internal class CustomStoreResourceLocator : ResourceLocator
{
    private static readonly DirectoryInfo ResourcesRootDirectory = 
        new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "resources" + Path.DirectorySeparatorChar));
    
    public override Resource GetResource(ResourceInfo resourceInfo)
    {
        var resourceLocator = new DefaultResourceLocator(new Uri(ResourcesRootDirectory.FullName));
        return resourceLocator.GetResource(resourceInfo);
    }
}