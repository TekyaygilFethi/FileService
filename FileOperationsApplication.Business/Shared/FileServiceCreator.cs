using FileOperationsApplication.Data.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Business.Shared
{
    public static class FileServiceCreator
    {
        public static IFileService GetFileService(FileDestinationServiceEnum fileDestination, IConfiguration configuration)
        {
            var namespacePath = GLOBALS.namespacePath.Replace("{0}", Enum.GetName(typeof(FileDestinationServiceEnum), fileDestination));
            Assembly assembly = Assembly.Load(namespacePath);
            var className = Enum.GetName(typeof(FileDestinationServiceEnum), fileDestination) + "FileService";
            var t = assembly.GetType(namespacePath + $".{className}");

            return t == default ? default : (IFileService)Activator.CreateInstance(t, configuration);
        }
    }
}
