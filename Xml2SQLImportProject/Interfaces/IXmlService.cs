using System.Data;
using Xml2SqlImport.Data.SPModels;
using Xml2SqlImport.Helpers.Responses;

namespace Xml2SqlImport.Interfaces
{
    public interface IXmlService
    {
        RetVal<DataSet> readXml(LocalXmlInfo xmlInfo);
    }
}