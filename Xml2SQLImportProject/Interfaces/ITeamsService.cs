using Xml2SqlImport.Helpers.Responses;

namespace Xml2SqlImport.Interfaces
{
    public interface ITeamsService
    {
        Task<RetVal> sendTeamsLastImportDates();
        Task<RetVal> sendTeamsLastKpiDateValues();
        Task<RetVal> sendTeamsMessage(string body, string title);
    }
}