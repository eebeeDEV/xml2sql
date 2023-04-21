using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{
    public class spIMPORT_GetNextFileInfo
    {
        public int? nextRunId { get; set; }
        public string? nextFileName { get; set; }
        public DateTime? nextDate { get; set; }
        public bool? noFileToImport { get; set; }
    }
}
