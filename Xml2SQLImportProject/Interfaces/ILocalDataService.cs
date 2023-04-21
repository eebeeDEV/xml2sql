using Xml2SqlImport.Data.Domain;
using Xml2SqlImport.Helpers.Enums;
using Xml2SqlImport.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2SqlImport.Data.SPModels;
using System.Data;

namespace Xml2SqlImport.Interfaces
{
    public interface ILocalDataService
    {
        Task<RetVal<spIMPORT_GetNextFileInfo>> getNextFileInfo();
        Task<RetVal> importXml(DataSet xmlModel, spIMPORT_GetNextFileInfo fileInfo, LocalXmlInfo xmlInfo);
        Task<RetVal> setPostImportValues(spIMPORT_GetNextFileInfo fileInfo, LocalXmlInfo xmlInfo);
        Task<RetVal> setBypassFileDate();
        Task<RetVal> calcKpiValues();
        Task<RetVal<List<spIMPORT_GetLastFileImportDates>>> getLastFileImportDates();
        Task<RetVal<List<spIMPORT_GetLast2KpiDateValues>>> getLastKpiDateValues();
    }
}
