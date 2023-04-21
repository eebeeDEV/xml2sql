using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2SqlImport.Interfaces;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Xml;
using Xml2SqlImport.Data.SPModels;
using Xml2SqlImport.Helpers.Responses;

namespace Xml2SqlImport.Services
{
    public class XmlService : IXmlService
    {
        private readonly ILocalDataService _dbService;
        private readonly IConfiguration _config;
        private readonly IAppSettings _settings;

        public XmlService(ILocalDataService dbService, IConfiguration config, IAppSettings settings)
        {
            _dbService = dbService;
            _config = config;
            _settings = settings;
        }



        public RetVal<DataSet> readXml(LocalXmlInfo xmlInfo)
        {
            var xmlDs = new DataSet("xmlFile");
            var ret = new RetVal<DataSet> { success = false, errorMessage = "Could not read the XML file" };
            try
            {
                var rd = XmlReader.Create(xmlInfo.localXmlFile!);

                
                xmlDs.ReadXmlSchema(xmlInfo.localXsdFile!);
                var clean = cleanDs(xmlDs);
                if (clean.success)
                {
                    xmlDs = clean.returnVal;
                    xmlDs!.ReadXml(rd);
                }

                rd.Dispose();
                ret.success = true;
                ret.errorMessage = null;
                ret.returnVal = xmlDs;

                // fill the tableList table
                var tblList = new utp_IMPORT_TableList();
                xmlDs!.Tables.OfType<DataTable>().Select(t => t.TableName).ToList().ForEach(t => tblList.Rows.Add($"{_settings.currentFile!.tablePrefix}{t}"));
                xmlInfo.tableList = tblList;
            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;
                Console.WriteLine(e.Message);
            }

            return ret;
        }


        // this function checks for duplicate fieldnames in the dataset
        // and removes them
        private RetVal<DataSet> cleanDs(DataSet ds)
        {
            var ret = new RetVal<DataSet> { success = false, errorMessage = "Could not read the XML file" };

            foreach (DataTable tbl in ds.Tables)
            {
                var lst = new Dictionary<string, int>();
                var dbl = new Dictionary<string, int>();
                // check if a column exist ing the table
                // if so, add it to the doubles dictionary
                foreach (DataColumn col in tbl.Columns)
                {
                    var nm = col.ColumnName;
                    var idx = tbl.Columns.IndexOf(nm);
                    var found = lst.ContainsKey(nm.ToLower());
                    if (found)
                    {
                        dbl.Add(nm.ToLower(), idx);
                    }
                    else
                    {
                        lst.Add(nm.ToLower(), idx);
                    }
                }
                // remove all the doubles from the list dictionary
                if (dbl.Count > 0)
                {
                    foreach (int idx in dbl.Values)
                    {
                        tbl.Columns.RemoveAt(idx);
                    }
                }
            }
            ret.returnVal = ds; 
            ret.success = true;
            ret.errorMessage = null;
            return ret;
        }




    }
}
