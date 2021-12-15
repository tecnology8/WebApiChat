using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiChat.Helpers
{
    public class AppSettings : IAppSettings
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public string ApplicationName { get; set; }
        public VersionInformation VersionInfo { get; set; }
        public string AppDescription { get; set; }
    }

    public interface IAppSettings
    {
        Dictionary<string, string> ConnectionStrings { get; set; }
        string ApplicationName { get; set; }
        VersionInformation VersionInfo { get; set; }
        string AppDescription { get; set; }
    }
    public class VersionInformation
    {
        public string VersionNumber { get; set; }
        public string Description { get; set; }
        public string VersionDate { get; set; }
        public bool Force { get; set; }
        public string BuildNumber { get; set; }
    }
}
