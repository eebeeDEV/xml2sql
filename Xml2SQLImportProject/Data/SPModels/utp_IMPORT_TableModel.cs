using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{

    public class spIMPORT_SyncXmlModelRec
    {
        public string? columnName { get; set; }
        public string? columnType { get; set; }
        public int? columnSize { get; set; }
    }

    public class utp_IMPORT_TableModel: DataTable
    {
        public utp_IMPORT_TableModel() {
            this.Columns.Add("columnName", typeof(string));
            this.Columns.Add("columnType", typeof(string));
            this.Columns.Add("columnSize", typeof(Int32));
        }
    }
}
