using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xml2SqlImport.Data.Context;
using Xml2SqlImport.Helpers.Enums;
using Xml2SqlImport.Helpers.Responses;
using Xml2SqlImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Xml2SqlImport.Services
{
    public class LogService : ILogService
    {

        private readonly LogDbContext _dbContext;
        private readonly IAppSettings _settings;

        public LogService(LogDbContext context, IAppSettings appSettings)
        {
            _dbContext = context;
            _settings = appSettings;
        }

        public async Task<TaskResult> logStepAsync(enumLogJobStep JobType, string stepDescr, string errorText, bool isSuccess, int recordAffected, DateTime stepStart, DateTime stepEnd, string errorType, int stepSeverity, bool isForTest, string tablePrefix)
        {
            try
            {
                int jobId = 23;
                string jobName = "";
                string stepSP = "";
                string appName = $"Xml2SqlImport - {tablePrefix}";
                string stepCategory = "";
                string respUsr = "Eric Bracke - Noel Wallez - Lieven Vanderghote";
                bool isDelta = false;

                switch (JobType)
                {
                    case enumLogJobStep.GET_NEXT_XML:
                        {
                            jobName = "GET NEXT XML";
                            stepSP = "spIMPORT_GetNextFileInfo";
                            stepCategory = "EXEC SP";
                            isDelta = false;
                            break;
                        }

                    case enumLogJobStep.COPY_XML_TO_LOCAL:
                        {
                            jobName = "COPY XML TO LOCAL";
                            stepCategory = "COPY FILE";
                            isDelta = false;
                            break;
                        }
                    case enumLogJobStep.READ_XML:
                        {
                            jobName = "READ XML";
                            stepCategory = "READ XML FILE";
                            isDelta = false;
                            break;
                        }
                    case enumLogJobStep.IMPORT_XML:
                        {
                            jobName = "IMPORT XML";
                            stepSP = "BulkCopy";
                            stepCategory = "BULKCOPY";
                            isDelta = true;
                            break;
                        }
                    case enumLogJobStep.IMPORT_XML_POST:
                        {
                            jobName = "IMPORT XML POST PROCESS";
                            stepSP = "spIMPORT_PostProcess";
                            stepCategory = "EXEC SP";
                            isDelta = true;
                            break;
                        }
                    case enumLogJobStep.RECAP_VALUES_CALC:
                        {
                            jobName = "RECAP VALUES CALC";
                            stepSP = "spIMPORT_CalcKpiValues";
                            stepCategory = "EXEC SP";
                            isDelta = true;
                            break;
                        }
                    case enumLogJobStep.DELETE_XML_LOCAL:
                        {
                            jobName = "DELETE XML LOCAL";                            
                            stepCategory = "DELETE FILE";
                            isDelta = true;
                            break;
                        }
                }


                var jIdParam = new SqlParameter { ParameterName = "@jobId", DbType = System.Data.DbType.Int32, Value = jobId };
                var jAppParam = new SqlParameter { ParameterName = "@appName", DbType = System.Data.DbType.String, Value = appName, Size = 100 };
                var jNameParam = new SqlParameter { ParameterName = "@jobName", DbType = System.Data.DbType.String, Value = jobName, Size = 50 };
                var jCatParam = new SqlParameter { ParameterName = "@stepCategory", DbType = System.Data.DbType.String, Value = stepCategory, Size = 50 };
                var jDescrParam = new SqlParameter { ParameterName = "@stepDescr", DbType = System.Data.DbType.String, Value = stepDescr, Size = 100 };
                var jSpParam = new SqlParameter { ParameterName = "@stepSP", DbType = System.Data.DbType.String, Value = stepSP, Size = 40 };
                var jStartParam = new SqlParameter { ParameterName = "@stepStart", DbType = System.Data.DbType.DateTime2, Value = stepStart };
                var jEndParam = new SqlParameter { ParameterName = "@stepEnd", DbType = System.Data.DbType.DateTime2, Value = stepEnd };
                var jRecsParam = new SqlParameter { ParameterName = "@recordAffected", DbType = System.Data.DbType.Int32, Value = recordAffected };
                var jDeltaParam = new SqlParameter { ParameterName = "@isDelta", DbType = System.Data.DbType.Boolean, Value = isDelta };
                var jTestParam = new SqlParameter { ParameterName = "@isForTest", DbType = System.Data.DbType.Boolean, Value = isForTest };
                var jIsSuccessParam = new SqlParameter { ParameterName = "@isSuccess", DbType = System.Data.DbType.Boolean, Value = isSuccess };
                var errTxtParam = new SqlParameter { ParameterName = "@stepError", DbType = System.Data.DbType.String, Value = errorText, Size = 800 };
                var errTypeParam = new SqlParameter { ParameterName = "@stepErrorType", DbType = System.Data.DbType.String, Value = errorType, Size = 50 };
                var errSeveParam = new SqlParameter { ParameterName = "@stepErrorSeverity", DbType = System.Data.DbType.Int32, Value = stepSeverity };
                var respUsrParam = new SqlParameter { ParameterName = "@responsibleUser", DbType = System.Data.DbType.String, Value = respUsr, Size = 50 };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };

                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXEC dbo.spCPT_LogJobStepV1");
                sB.AppendLine("@jobId, ");
                sB.AppendLine("@appName, ");
                sB.AppendLine("@jobName, ");
                sB.AppendLine("@stepCategory, ");
                sB.AppendLine("@stepDescr, ");
                sB.AppendLine("@stepSP, ");
                sB.AppendLine("@stepStart, ");
                sB.AppendLine("@stepEnd, ");
                sB.AppendLine("@recordAffected, ");
                sB.AppendLine("@isDelta, ");
                sB.AppendLine("@isForTest, ");
                sB.AppendLine("@isSuccess, ");
                sB.AppendLine("@stepError, ");
                sB.AppendLine("@stepErrorType, ");
                sB.AppendLine("@stepErrorSeverity, ");
                sB.AppendLine("@responsibleUser, ");
                sB.AppendLine("@Success OUT");

                await _dbContext.Database.ExecuteSqlRawAsync(sB.ToString(), jIdParam, jAppParam, jNameParam, jCatParam, jDescrParam, jSpParam,
                    jStartParam, jEndParam, jRecsParam, jDeltaParam, jTestParam, jIsSuccessParam, errTxtParam, errTypeParam, errSeveParam, respUsrParam, successParam);
                // get the value of the output paramater
                var result = (bool)successParam.Value;
                return new TaskResult { success = result };
            }
            catch (Exception e)
            {

                return new TaskResult { success = false, error = e.Message };
            }
        }

        public async Task<RetVal<bool>> isDateToBypass(DateTime theDate, bool isMissing)

        {

            var ret = new RetVal<bool> { success = false, returnVal = false, errorMessage = "Something went wrong" };

            try
            {
                var dateParam = new SqlParameter { ParameterName = "@TheDate", DbType = System.Data.DbType.DateTime, Value = theDate };
                var weekEndParam = new SqlParameter { ParameterName = "@IsWeekend", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var holydayParam = new SqlParameter { ParameterName = "@IsHolyday", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };

                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXEC dbo.spCPT_LogGetDateDimInfo ");
                sB.AppendLine("@TheDate, ");
                sB.AppendLine("@IsWeekend OUT, ");
                sB.AppendLine("@IsHolyday OUT, ");
                sB.AppendLine("@Success OUT");

                await _dbContext.Database.ExecuteSqlRawAsync(sB.ToString(), dateParam, weekEndParam, holydayParam, successParam);

                // get the value of the output paramater
                var success = (bool)successParam.Value;
                if (!success)
                {
                    ret.errorMessage = "Could not fetch the date dim data";
                    return ret;
                }

                var isWeekend = (bool)weekEndParam.Value;
                var isHoliday = (bool)holydayParam.Value;

                if (isMissing)
                {
                    if (isWeekend && _settings.currentFile!.bypassMissingFileWeekend) ret.returnVal = true;
                    if (isHoliday && _settings.currentFile!.bypassMissingFileHolyday) ret.returnVal = true;
                    if (!isWeekend && !isHoliday && _settings.currentFile!.bypassMissingFileWeek) ret.returnVal = true;
                }
                else
                {
                    if (isWeekend && _settings.currentFile!.bypassEmptyFileWeekend) ret.returnVal = true;
                    if (isHoliday && _settings.currentFile!.bypassEmptyFileHolyday) ret.returnVal = true;
                    if (!isWeekend && !isHoliday && _settings.currentFile!.bypassEmptyFileWeek) ret.returnVal = true;
                }

                ret.errorMessage = null;
                ret.success = true;
                return ret;
            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;

            }

            return ret;


        }


    }
}
