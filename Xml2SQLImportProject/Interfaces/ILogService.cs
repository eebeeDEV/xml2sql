using Xml2SqlImport.Helpers.Enums;
using Xml2SqlImport.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Interfaces
{
    public interface ILogService
    {        
        Task<RetVal<bool>> isDateToBypass(DateTime theDate, bool isMissing);
        Task<TaskResult> logStepAsync(enumLogJobStep JobStep, string stepDescr, string errorText, bool isSuccess, int recordAffected, DateTime stepStart, DateTime stepEnd, string errorType, int stepSeverity, bool isForTest, string tablePrefix);
    }
}
