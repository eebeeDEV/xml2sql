using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{
    public class spGetTableFieldsRec
    {
        public string? fieldName { get; set; }
        public Byte? fieldTypeId { get; set; }
        public string? fieldType { get; set; }
        public Int16? fieldSize { get; set; }
    }
}
