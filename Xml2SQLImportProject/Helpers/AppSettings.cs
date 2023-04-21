using Xml2SqlImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers
{
    public class AppSettings : IAppSettings
    {
        public List<FileSettings>? FileSettings { get; set; }
        public FileSettings? currentFile { get; set; }
        public string? teamsWebhook { get; set; }
    }

    public class FileSettings
    {
        public string? tablePrefix { get; set; }
        public string? localSchema { get; set; }
        public string? logSchema { get; set; }
        public string? fileBaseName { get; set; }
        public string? fileImportFolder { get; set; }
        public string? whichCase { get; set; }
        public string? brrddelPartitionFields { get; set; }
        public string? brrddelOrderByFields { get; set; }
        public bool bypassEmptyFileHolyday { get; set; }
        public bool bypassEmptyFileWeekend { get; set; }
        public bool bypassEmptyFileWeek { get; set; }
        public bool bypassMissingFileHolyday { get; set; }
        public bool bypassMissingFileWeekend { get; set; }
        public bool bypassMissingFileWeek { get; set; }
        public bool isKpiFile { get; set; }

    }
}
