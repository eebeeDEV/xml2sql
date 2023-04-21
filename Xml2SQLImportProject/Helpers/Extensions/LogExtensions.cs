using Xml2SqlImport.Helpers.Responses;
using Xml2SqlImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers.Extensions
{
    public static class LogExtensions
    {
        private static ILogService? _log;
        public static void LogExtensionsConfigure(ILogService log)
        {
            _log = log;
        }

        /// <summary>
        /// With generic type
        /// This extension logs the result of the RetVal object
        /// Returns BaseResult<T>.Success
        /// </summary>
        /// <typeparam name="T">Generic type must be a class</typeparam>        
        /// <param name="logSuccess">If false, only failed result is logged</param>
        /// <returns>bool</returns>
        public static Task<bool> LogNow<T>(this TaskResult<T> result, bool logSuccess, DateTime startTime, DateTime endTime) where T : class
        {

            // as this is a static function, we cannot make it async
            // so we need to create a async task in the body
            return (Task<bool>)Task.Run(async () =>
            {
                if (!result.success)
                {
                    //await _log!.logStepAsync(result.logJobType, result.logStepDescr!, result.error!, result.success, 0, startTime, endTime, result.errorType!);
                }
                else if (logSuccess)
                {
                    //await _log!.logStepAsync(result.logJobType, result.logStepDescr!, "", result.success, result.returnValue!.Count(), startTime, endTime, "");
                }
                return result.success;
            });
        }

        /// <summary>
        /// Without generic type
        /// This extension logs the result of the RetVal object
        /// Returns BaseResult.Success
        /// </summary>           
        /// <param name="logSuccess">If false, only failed result is logged</param>
        /// <returns>bool</returns>
        public static Task<bool> LogNow(this TaskResult result, bool logSuccess, DateTime startTime, DateTime endTime) 
        {

            // as this is a static function, we cannot make it async
            // so we need to create a async task in the body
            return (Task<bool>)Task.Run(async () =>
            {
                if (!result.success)
                {
                    //await _log!.logStepAsync(result.logJobType, result.logStepDescr!, result.error!, result.success, 0, startTime, endTime, result.errorType!);
                }
                else if (logSuccess)
                {
                }
                return result.success;
            });
        }

    }
}
