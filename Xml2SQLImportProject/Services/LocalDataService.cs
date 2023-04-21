using Xml2SqlImport.Interfaces;
using Xml2SqlImport.Data.Context;
using Xml2SqlImport.Helpers;
using Xml2SqlImport.Data.SPModels;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Runtime;
using Xml2SqlImport.Helpers.Responses;



namespace Xml2SqlImport.Services
{
    public class LocalDataService : ILocalDataService
    {
        private readonly LocalDbContext _db;
        private readonly IAppSettings _appSettings;


        public LocalDataService(LocalDbContext lDb, IAppSettings appSettings)
        {
            _db = lDb;
            _appSettings = appSettings;

        }


        public async Task<RetVal> importXml(DataSet xmlModel, spIMPORT_GetNextFileInfo fileInfo, LocalXmlInfo xmlInfo)
        {
            var ret = new RetVal { success = false, errorMessage = "Could not sync the datamodels" };
            var x = 0;
            var allT = xmlModel.Tables.Count;

            var tblList = new utp_IMPORT_TableList();

            foreach (DataTable tbl in xmlModel.Tables)
            {
                x++;
                Console.WriteLine($"START TABLE: {DateTime.Now} - TABLE: {tbl.TableName} - {x}/{allT}");

                //if(tbl.TableName != "pyFlowParameters")
                //{
                //    Console.WriteLine($"BYPASS TABLE: {tbl.TableName} - {x}/{allT}");
                //    continue;
                //}

                // if the table has no data, do not import
                if (tbl.Rows.Count > 0)
                {
                    var tblName = $"{_appSettings.currentFile!.tablePrefix}{tbl.TableName}";
                    // get the table model
                    var xmlTable = _getXmlModelFromTable(tbl);
                    if (!xmlTable.success)
                    {
                        ret.errorMessage = xmlTable.errorMessage;
                        return ret;
                    }

                    // create the table if it doesn't exists
                    var modelRet = await _syncXmlModelWithDb(tblName, xmlTable.returnVal!, xmlInfo, fileInfo);
                    if (!modelRet.success)
                    {
                        ret.errorMessage = modelRet.errorMessage;
                        return ret;
                    }

                    // if the table is not bypassed, _inject the data
                    if (modelRet.returnVal!.Count > 0)
                    {
                        var injRet = _inject(tbl, modelRet.returnVal, fileInfo, xmlInfo);
                        if (!injRet.success)
                        {
                            Console.WriteLine($"Could not inject table: {tblName} - error:{injRet.errorMessage}");
                            ret.errorMessage = $"Could not inject table: {tblName} - error:{injRet.errorMessage}";                            
                            return ret;
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"TABLE: {tbl.TableName} IS EMPTY!!");
                }

            }

            // log the table list changes            
            var tblRet = await _syncXmlTableNamesWithDb(xmlInfo, fileInfo);
            if (!tblRet.success)
            {
                ret.errorMessage = tblRet.errorMessage;
                return ret;
            }


            Console.WriteLine($"FILE END: {DateTime.Now}");
            ret.success = true;
            ret.errorMessage = "";
            return ret;
        }





        public async Task<RetVal<spIMPORT_GetNextFileInfo>> getNextFileInfo()
        {

            var ret = new RetVal<spIMPORT_GetNextFileInfo> { success = false, errorMessage = "Could not get the fieldnames", returnInt = -1 };

            try
            {
                var baseParam = new SqlParameter { ParameterName = "@FileBaseName", DbType = System.Data.DbType.String, Size = 100, Value = _appSettings.currentFile!.fileBaseName };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_GetNextFileInfo ");
                sB.AppendLine("@FileBaseName,");
                //sB.AppendLine("@IgnoreEmptyFileWeekday,");
                //sB.AppendLine("@IgnoreEmptyFileHolyday,");
                //sB.AppendLine("@IgnoreEmptyFileWeekend,");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.FileInfo!.FromSqlRaw(sB.ToString(), baseParam, successParam, errParam).ToListAsync();


                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;


                ret.errorMessage = err;
                ret.success = success;
                ret.returnVal = r.FirstOrDefault();
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }


        public async Task<RetVal> setPostImportValues(spIMPORT_GetNextFileInfo fileInfo, LocalXmlInfo xmlInfo)
        {

            var ret = new RetVal { success = false, errorMessage = "Could not get the fieldnames", returnInt = -1 };

            try
            {
                var baseParam = new SqlParameter { ParameterName = "@FileBaseName", DbType = System.Data.DbType.String, Size = 100, Value = _appSettings.currentFile!.fileBaseName };               
                var fileParam = new SqlParameter { ParameterName = "@FileName", DbType = System.Data.DbType.String, Size = 100, Value = xmlInfo.localXmlFileName };
                var dateParam = new SqlParameter { ParameterName = "@FileDate", DbType = System.Data.DbType.Date, Value = fileInfo.nextDate };
                var runParam = new SqlParameter { ParameterName = "@RunId", DbType = System.Data.DbType.Int32, Value = fileInfo.nextRunId };
                var schemaParam = new SqlParameter { ParameterName = "@SchemaName", DbType = System.Data.DbType.String, Size = 20, Value = _appSettings.currentFile!.localSchema };
                var whichParam = new SqlParameter { ParameterName = "@TablePrefix", DbType = System.Data.DbType.String, Size = 60, Value = _appSettings.currentFile!.tablePrefix };
                var partitionParam = new SqlParameter { ParameterName = "@BrrddelPartitionFields", DbType = System.Data.DbType.String, Size = 400, Value = _appSettings.currentFile!.brrddelPartitionFields };
                var orderParam = new SqlParameter { ParameterName = "@BrrddelOrderByFields", DbType = System.Data.DbType.String, Size = 400, Value = _appSettings.currentFile!.brrddelOrderByFields };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_PostProcess ");
                sB.AppendLine("@FileBaseName,");
                sB.AppendLine("@FileName,");
                sB.AppendLine("@FileDate,");
                sB.AppendLine("@RunId,");
                sB.AppendLine("@SchemaName,");
                sB.AppendLine("@TablePrefix,");
                sB.AppendLine("@BrrddelPartitionFields,");
                sB.AppendLine("@BrrddelOrderByFields,");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.Database.ExecuteSqlRawAsync(sB.ToString(), baseParam, fileParam, dateParam, runParam, schemaParam, whichParam, partitionParam, orderParam, successParam, errParam);

                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;


                ret.errorMessage = err;
                ret.success = success;
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }


        public async Task<RetVal> calcKpiValues()
        {

            var ret = new RetVal { success = false, errorMessage = "Could not execute the SP", returnInt = -1 };

            try
            {
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_PostProcess_CalcKpiFromKpiFile ");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.Database.ExecuteSqlRawAsync(sB.ToString(), successParam, errParam);

                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;


                ret.errorMessage = err;
                ret.success = success;
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }

        public async Task<RetVal<List<spIMPORT_GetLastFileImportDates>>> getLastFileImportDates()
        {

            var ret = new RetVal<List<spIMPORT_GetLastFileImportDates>> { success = false, errorMessage = "Could not execute the SP", returnInt = -1 };

            try
            {
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_GetLastFileImportDates ");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();                
                var r = await _db.LastImportFiles!.FromSqlRaw(sB.ToString(), successParam, errParam).ToListAsync();

                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;

                if(success)
                {
                    ret.returnVal = r;
                }

                ret.errorMessage = err;
                ret.success = success;
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }

        public async Task<RetVal<List<spIMPORT_GetLast2KpiDateValues>>> getLastKpiDateValues()
        {

            var ret = new RetVal<List<spIMPORT_GetLast2KpiDateValues>> { success = false, errorMessage = "Could not execute the SP", returnInt = -1 };

            try
            {
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_GetLast2KpiDateValues ");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.Last2KpiDateValues!.FromSqlRaw(sB.ToString(), successParam, errParam).ToListAsync();

                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;

                if (success)
                {
                    ret.returnVal = r;
                }

                ret.errorMessage = err;
                ret.success = success;
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }

        private RetVal<bool> _inject(DataTable tbl, List<spIMPORT_SyncXmlModelRec> flds, spIMPORT_GetNextFileInfo fileInfo, LocalXmlInfo xmlInfo)
        {
            var ret = new RetVal<bool> { success = false, errorMessage = "Could not inject the data", returnVal = false };

            try
            {
                using (SqlConnection connection = new SqlConnection(_db.Database.GetDbConnection().ConnectionString))
                {
                    connection.Open();

                    // add the default fields values
                    //var dte = fileInfo!.nextDate.ToString("d", CultureInfo.CreateSpecificCulture("en-US"));
                    var dte = string.Format("{0:yyyy-MM-dd}", fileInfo!.nextDate);
                    tbl.Columns.Add(new DataColumn { ColumnName = "fileName", DataType = typeof(string), Expression = $"'{xmlInfo.localXmlFileName}'" });
                    tbl.Columns.Add(new DataColumn { ColumnName = "fileDate", DataType = typeof(DateTime), Expression = $"'{dte}'" });
                    tbl.Columns.Add(new DataColumn { ColumnName = "runId", DataType = typeof(int), Expression = $"{fileInfo.nextRunId ?? 999}" });

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.BulkCopyTimeout = 3600;
                        bulkCopy.DestinationTableName = $"{_appSettings.currentFile!.tablePrefix}{tbl.TableName}";
                        // map the newly created columns
                        bulkCopy.ColumnMappings.Add("fileDate", "fileDate");
                        bulkCopy.ColumnMappings.Add("fileName", "fileName");
                        bulkCopy.ColumnMappings.Add("runId", "runId");

                        // add the xml mappings
                        foreach (spIMPORT_SyncXmlModelRec col in flds)
                        {
                            bulkCopy.ColumnMappings.Add($"{col.columnName}", $"{col.columnName}");
                        }

                        bulkCopy.WriteToServer(tbl);
                    }
                }
                ret.errorMessage = null;
                ret.success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ret.success = false;
                ret.errorMessage = e.Message;
            }





            return ret;
        }



        private RetVal<utp_IMPORT_TableModel> _getXmlModelFromTable(DataTable tbl)
        {
            var ret = new RetVal<utp_IMPORT_TableModel> { success = false, errorMessage = "Table contains no data" };
            var retTbl = new utp_IMPORT_TableModel();

            try
            {
                ret.success = true;
                ret.errorMessage = null;

                tbl = _cleanSingleContent(tbl);

                foreach (DataColumn col in tbl.Columns)
                {

                    var rw = retTbl.NewRow();
                    rw["columnName"] = col.ColumnName;

                    if (col.DataType == System.Type.GetType("System.String"))
                    {

                        var vw = new DataView(tbl);
                        var cTbl = vw.ToTable(false, col.ColumnName);

                        string colName = $"{col.ColumnName}_lencalc";
                        var colLen = cTbl.Columns.Add(colName, typeof(Int32), $"Len(ISNULL({col.ColumnName}, ''))");
                        var maxWidth = cTbl.AsEnumerable().Max(r => (Int32)r[colName]);

                        if (maxWidth > -1)
                        {
                            maxWidth = Globals.calcFieldSize(maxWidth);
                            if (maxWidth > 4000) rw["columnType"] = "varchar(MAX)"; else rw["columnType"] = $"varchar({maxWidth})";
                            rw["columnSize"] = maxWidth;

                        }

                        maxWidth = -1;
                    }

                    else if (col.DataType == System.Type.GetType("System.Int32")) rw["columnType"] = "int";
                    else if (col.DataType == System.Type.GetType("System.Int64")) rw["columnType"] = "bigint";
                    else if (col.DataType == System.Type.GetType("System.DateTime")) rw["columnType"] = "datetime2(7)";
                    else if (col.DataType == System.Type.GetType("System.Boolean")) rw["columnType"] = "bit";
                    else if (col.DataType == System.Type.GetType("System.Double")) rw["columnType"] = "float";
                    else if (col.DataType == System.Type.GetType("System.Decimal")) rw["columnType"] = "float";
                    else rw["columnType"] = "sql_variant";

                    retTbl.Rows.Add(rw);
                }
                // return the table
                ret.returnVal = retTbl;
                ret.errorMessage = null;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.success = false;
                ret.errorMessage = e.Message;
            }


            return ret;

        }


        private DataTable _cleanSingleContent(DataTable tbl)
        {
            var lst = new List<String>();

            foreach (DataColumn dataColumn in tbl.Columns)
            {
                if (dataColumn.ColumnMapping.ToString() == "SimpleContent")
                {
                    dataColumn.ColumnMapping = MappingType.Attribute;
                }
            }

            return tbl;
        }


        private async Task<RetVal<List<spIMPORT_SyncXmlModelRec>>> _syncXmlModelWithDb(string tableName, utp_IMPORT_TableModel xmlTable, LocalXmlInfo xmlInfo, spIMPORT_GetNextFileInfo fileInfo)
        {

            var ret = new RetVal<List<spIMPORT_SyncXmlModelRec>> { success = false, errorMessage = "Could not get the fieldnames", returnInt = -1 };

            try
            {

                var schemaParam = new SqlParameter { ParameterName = "@SchemaName", DbType = System.Data.DbType.String, Size = 20, Value = _appSettings.currentFile!.localSchema };
                var nameParam = new SqlParameter { ParameterName = "@TableName", DbType = System.Data.DbType.String, Size = 100, Value = tableName };
                var tblParam = new SqlParameter { ParameterName = "@TableModel", SqlDbType = SqlDbType.Structured, TypeName = "dbo.utp_IMPORT_TableModel", Value = xmlTable, Direction = ParameterDirection.Input };
                var fileParam = new SqlParameter { ParameterName = "@FileName", DbType = System.Data.DbType.String, Size = 100, Value = xmlInfo.localXmlFileName };
                var dateParam = new SqlParameter { ParameterName = "@FileDate", DbType = System.Data.DbType.Date, Value = xmlInfo.localXmlFileDate };
                var baseParam = new SqlParameter { ParameterName = "@FileBaseName", DbType = System.Data.DbType.String, Size = 100, Value = _appSettings.currentFile!.fileBaseName };
                var runParam = new SqlParameter { ParameterName = "@RunId", DbType = System.Data.DbType.Int32, Value = fileInfo.nextRunId };
                var whichParam = new SqlParameter { ParameterName = "@WhichCase", DbType = System.Data.DbType.String, Size = 50, Value = _appSettings.currentFile!.whichCase };
                var dropParam = new SqlParameter { ParameterName = "@DropIfExists", DbType = System.Data.DbType.Boolean, Value = false };
                var existsParam = new SqlParameter { ParameterName = "@TableExists", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };


                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_SyncXmlTableModelWithDb ");
                sB.AppendLine("@SchemaName,");
                sB.AppendLine("@TableName,");
                sB.AppendLine("@TableModel,");
                sB.AppendLine("@FileName,");
                sB.AppendLine("@FileDate,");
                sB.AppendLine("@FileBaseName,");
                sB.AppendLine("@RunId,");
                sB.AppendLine("@WhichCase,");
                sB.AppendLine("@DropIfExists,");
                sB.AppendLine("@TableExists OUT,");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.TableModel!.FromSqlRaw(sB.ToString(), schemaParam, nameParam, tblParam, fileParam, dateParam, baseParam, runParam, whichParam, dropParam, existsParam, successParam, errParam).ToListAsync();

                var success = (bool)successParam.Value;
                var exists = (bool)existsParam.Value;
                var err = (string)errParam.Value;


                ret.errorMessage = err;
                ret.success = success;
                ret.returnVal = r;
                if (exists) ret.errorMessage = "Table exists";
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }


        private async Task<RetVal> _syncXmlTableNamesWithDb(LocalXmlInfo xmlInfo, spIMPORT_GetNextFileInfo fileInfo)
        {

            var ret = new RetVal { success = false, errorMessage = "Could not sync the tablenames" };

            try
            {
                var tblParam = new SqlParameter { ParameterName = "@TableNames", SqlDbType = SqlDbType.Structured, TypeName = "dbo.utp_IMPORT_TableList", Value = xmlInfo.tableList, Direction = ParameterDirection.Input };
                var whichParam = new SqlParameter { ParameterName = "@WhichCase", DbType = System.Data.DbType.String, Size = 50, Value = _appSettings.currentFile!.whichCase };
                var fileParam = new SqlParameter { ParameterName = "@FileName", DbType = System.Data.DbType.String, Size = 100, Value = xmlInfo.localXmlFileName };
                var dateParam = new SqlParameter { ParameterName = "@FileDate", DbType = System.Data.DbType.Date, Value = xmlInfo.localXmlFileDate };
                var runParam = new SqlParameter { ParameterName = "@RunId", DbType = System.Data.DbType.Int32, Value = fileInfo.nextRunId };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };

                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXECUTE dbo.spIMPORT_SyncXmlTableNamesWithDb ");
                sB.AppendLine("@TableNames,");
                sB.AppendLine("@WhichCase,");
                sB.AppendLine("@FileName,");
                sB.AppendLine("@FileDate,");
                sB.AppendLine("@RunId,");
                sB.AppendLine("@Success OUT,");
                sB.AppendLine("@ErrorMessage OUT");

                var sql = sB.ToString();
                var r = await _db.Database.ExecuteSqlRawAsync(sB.ToString(), tblParam, whichParam, fileParam, dateParam, runParam, successParam, errParam);

                var success = (bool)successParam.Value;
                var err = (string)errParam.Value;

                ret.success = success;
                return ret;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                ret.errorMessage = e.Message;
            }

            return ret;
        }

        public async Task<RetVal> setBypassFileDate()
        {

            var ret = new RetVal { success = false, errorMessage = "Something went wrong" };

            try
            {
                var baseParam = new SqlParameter { ParameterName = "@FileBaseName", DbType = System.Data.DbType.String, Size = 100, Value = _appSettings.currentFile!.fileBaseName };
                var successParam = new SqlParameter { ParameterName = "@Success", DbType = System.Data.DbType.Boolean, Direction = System.Data.ParameterDirection.Output };
                var errParam = new SqlParameter { ParameterName = "@ErrorMessage", DbType = System.Data.DbType.String, Size = -1, Direction = System.Data.ParameterDirection.Output };

                StringBuilder sB = new StringBuilder();
                sB.AppendLine("EXEC dbo.spIMPORT_SaveBypassDate ");
                sB.AppendLine("@FileBaseName, ");
                sB.AppendLine("@Success OUT, ");
                sB.AppendLine("@ErrorMessage OUT ");

                await _db.Database.ExecuteSqlRawAsync(sB.ToString(), baseParam, successParam, errParam);

                // get the value of the output paramater
                ret.success = (bool)successParam.Value;
                ret.errorMessage = (string)errParam.Value;
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
