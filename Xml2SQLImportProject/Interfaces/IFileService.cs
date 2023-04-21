using Xml2SqlImport.Data.SPModels;
using Xml2SqlImport.Helpers.Responses;

namespace Xml2SqlImport.Interfaces
{
    public interface IFileService
    {
        Task<RetVal<LocalXmlInfo>> copyXml(string fileName, DateTime xmlDate);
        RetVal copyXsdFiles();
        RetVal deleteLocalXml(string xmlFile);        
    }
}