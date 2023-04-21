using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2SqlImport.Data.SPModels;

namespace Xml2SqlImport.Helpers.Responses
{
    public class LocalXmlInfo
    {
        public string? localXmlFile { get; set; }
        public string? localXsdFile { get; set; }
        public string? localXmlFileName { get; set; }
        public DateTime? localXmlFileDate { get; set; }
        public utp_IMPORT_TableList? tableList { get; set; }
        public FileMissingOrBypass? bypassInfo { get; set; }

    }
}
