using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers.Responses
{
    public class FileMissingOrBypass
    {
        public bool success { get; set; }
        public string? errorMessage { get; set; }
        public bool isMissing { get; set; }
        public bool isEmpty { get; set; }
        public bool isBypassed { get; set; }
        public bool isOk { get; set; }

    }
}
