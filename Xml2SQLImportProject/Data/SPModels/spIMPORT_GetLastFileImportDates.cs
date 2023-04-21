using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{
    public class spIMPORT_GetLastFileImportDates
    {
        public string? fileName { get; set; }
        public string? lastDate { get; set; }
        public string? daysPassed { get; set; }
    }
}
