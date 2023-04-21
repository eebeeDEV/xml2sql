using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2SqlImport.Helpers;

namespace Xml2SqlImport.Interfaces
{
    public interface IAppSettings
    {
        public List<FileSettings>? FileSettings { get; set; }
        public FileSettings? currentFile { get; set; }
        public string? teamsWebhook { get; set; }
    }


}
