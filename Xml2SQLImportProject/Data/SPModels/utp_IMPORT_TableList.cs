using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{

    public class utp_IMPORT_TableList : DataTable
    {
        public utp_IMPORT_TableList()
        {
            this.Columns.Add("tableName", typeof(string));

        }
    }
}
