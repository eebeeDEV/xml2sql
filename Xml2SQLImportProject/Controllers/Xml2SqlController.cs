using Xml2SqlImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xml2SqlImport.Helpers;
using Xml2SqlImport.Helpers.Enums;
using AutoMapper;
using Xml2SqlImport.Data.Domain;
using Xml2SqlImport.Data.CsVModels;
using System.Data;
using System.IO;

namespace Xml2SqlImport.Controllers
{
    public class Xml2SqlController : IXml2SqlController
    {
        private readonly ILocalDataService _db;
        private readonly IMapper _map;
        private readonly IXmlService _xml;
        private readonly IConfiguration _config;
        private readonly IFileService _file;
        private readonly ILogService _log;
        private readonly IAppSettings _settings;
        private readonly ITeamsService _teams;

        public Xml2SqlController(ILocalDataService lContext, IMapper mapper, IXmlService xml, IConfiguration config, IFileService file, ILogService log, IAppSettings settings, ITeamsService teams)
        {
            _db = lContext;
            _map = mapper;
            _xml = xml;
            _config = config;
            _file = file;
            _log = log;
            _settings = settings;
            _teams = teams;
        }

        public async Task sendMessage()
        {
            await _teams.sendTeamsMessage("this is a test message", "Bix import");
        }


        public void copyXsdFiles()
        {
            _file.copyXsdFiles();
        }


        public async Task sendTeamsLastImportDates()
        {
            var result = await _teams.sendTeamsLastImportDates();
            if(!result.success) {
                Console.WriteLine($"Could not send Teams message: {result.errorMessage}");
            } else
            {
                Console.WriteLine("Teams message sent successfully");
            }
        }

        public async Task sendTeamsLastKpiDateValues()
        {
            var result = await _teams.sendTeamsLastKpiDateValues();
            if (!result.success)
            {
                Console.WriteLine($"Could not send Teams message: {result.errorMessage}");
            }
            else
            {
                Console.WriteLine("Teams message sent successfully");
            }
        }
        public async Task injectXml()
        {

            Console.WriteLine($"PROGRAM START: {DateTime.Now}");

            // as the appConfig can contain multiple fileconfigs
            // loop through all of them and import each file
            var fileCnt = _settings.FileSettings!.Count();

            for (int i = 0; i < fileCnt; i++)
            {
                _settings.currentFile = _settings.FileSettings![i];

                var runNow = true;
                
                do
                {

                    // get the next file
                    DateTime stepStart = DateTime.Now;
                    var fileInfo = await _db.getNextFileInfo();
                    DateTime stepEnd = DateTime.Now;
                    
                    if (!fileInfo.success)
                    {
                        await _log.logStepAsync(enumLogJobStep.GET_NEXT_XML, "Get next XML fileInfo", fileInfo.errorMessage!, fileInfo.success, 0, stepStart, stepEnd, "Could not get the fileInfo", (int)enumSeverity.MID, Globals.isForTest, _settings.currentFile!.tablePrefix!);
                        Console.WriteLine($"Could not get the fileInfo: {fileInfo.errorMessage}");
                        break;
                    }
                    if ((bool)fileInfo.returnVal!.noFileToImport!)
                    {
                        Console.WriteLine($"******* All files for {_settings.currentFile!.tablePrefix!} imported *******");
                        runNow = false;
                        continue;
                    }

                    // copy the xml + xsd files to the local app folder
                    // and check if file is missing or empty and if it should be bypassed
                    stepStart= DateTime.Now;
                    var copied = await _file.copyXml(fileInfo.returnVal.nextFileName!, (DateTime)fileInfo.returnVal.nextDate!);
                    stepEnd = DateTime.Now;                    


                    // if file is missing or empty but must be bypassed
                    if (!copied.success && copied.returnVal!.bypassInfo!.isBypassed)
                    {
                        var bypassDate = await _db.setBypassFileDate();
                        if (!bypassDate.success)
                        {
                            await _log.logStepAsync(enumLogJobStep.COPY_XML_TO_LOCAL, "Copying XML to local drive", copied.errorMessage!, copied.success, 0, stepStart, stepEnd, "Could not set the bypass date for file", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                            Console.WriteLine($"Could not set the bypass date for file: {fileInfo.returnVal.nextFileName!}!");
                            runNow = false;
                            continue;
                        }
                        await _log.logStepAsync(enumLogJobStep.GET_NEXT_XML, "Copying XML to local drive - file bypassed", copied.errorMessage!, copied.success, 0, stepStart, stepEnd, "", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        // if file is bypassed, get the next one
                        Console.WriteLine($"File: {fileInfo.returnVal.nextFileName!} is missing: {copied.returnVal!.bypassInfo.isMissing} is empty: {copied.returnVal!.bypassInfo.isEmpty} is bypassed!");
                        continue;
                    }

                    // if file is missing or empty but must not be bypassed
                    if (!copied.success)
                    {
                        if (!copied.returnVal!.bypassInfo!.isOk && !copied.returnVal.bypassInfo.isBypassed)
                        {
                            Console.WriteLine($"File: {fileInfo.returnVal.nextFileName!} is missing: {copied.returnVal!.bypassInfo.isMissing} is empty: {copied.returnVal!.bypassInfo.isEmpty}, CANNOT be bypassed!");
                            await _log.logStepAsync(enumLogJobStep.COPY_XML_TO_LOCAL, "Copying XML to local drive", copied.errorMessage!, copied.success, 0, stepStart, stepEnd, "File is missing or empty and must be bypassed", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        }
                        else
                        {
                            Console.WriteLine($"File: {fileInfo.returnVal.nextFileName!} could not be copied.");
                            await _log.logStepAsync(enumLogJobStep.COPY_XML_TO_LOCAL, "Copying XML to local drive", copied.errorMessage!, copied.success, 0, stepStart, stepEnd, "Could not copy the file", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        }
                        runNow = false;
                        continue;
                    }

                    

                    // read the local xml file
                    stepStart = DateTime.Now;
                    var read = _xml.readXml(copied.returnVal!);
                    stepEnd = DateTime.Now;
                    if (!read.success)
                    {
                        await _log.logStepAsync(enumLogJobStep.READ_XML, "Read XML file", read.errorMessage!, read.success, 0, stepStart, stepEnd, "Could not read the xml file", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        Console.WriteLine($"Could not read the xml file: {read.errorMessage}");
                        break;
                    }

                    // _inject the data into SQL db
                    stepStart= DateTime.Now;
                    var inject = await _db.importXml(read.returnVal!, fileInfo.returnVal, copied.returnVal!);
                    stepEnd= DateTime.Now;
                    if (!inject.success)
                    {
                        await _log.logStepAsync(enumLogJobStep.IMPORT_XML, "Save data into DB", inject.errorMessage!, inject.success, 0, stepStart, stepEnd, "Could not inject the data", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        Console.WriteLine($"Could not inject the data into the SQL db: {inject.errorMessage}");
                        break;
                    }


                    // do the _inject post-process 
                    stepStart= DateTime.Now;
                    var post = await _db.setPostImportValues(fileInfo.returnVal,copied.returnVal!);
                    stepEnd= DateTime.Now;
                    if (!post.success)
                    {
                        await _log.logStepAsync(enumLogJobStep.IMPORT_XML_POST, "Save data into DB - post process", post.errorMessage!, post.success, 0, stepStart, stepEnd, "Could not run the spIMPORT_PostProcess", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        Console.WriteLine($"Could not run the spIMPORT_PostProcess in the SQL db: {post.errorMessage}");
                        break;
                    }

                    if (_settings.currentFile.isKpiFile)
                    {
                        stepStart= DateTime.Now;
                        var kpi = await _db.calcKpiValues();
                        stepEnd= DateTime.Now;
                        if (!kpi.success)
                        {
                            await _log.logStepAsync(enumLogJobStep.RECAP_VALUES_CALC, "Save data into DB - kpi values", post.errorMessage!, post.success, 0, stepStart, stepEnd, "Could not calculate the kpi values", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                            Console.WriteLine($"Could not calculate the kpi values: {kpi.errorMessage}");
                            break;
                        }
                    }

                    // delete the local xml files 
                    stepStart = DateTime.Now;
                    var del = _file.deleteLocalXml(fileInfo.returnVal.nextFileName!);
                    stepEnd= DateTime.Now;
                    if (!del.success)
                    {
                        await _log.logStepAsync(enumLogJobStep.DELETE_XML_LOCAL, "Save data into DB - kpi values", del.errorMessage!, del.success, 0, stepStart, stepEnd, "Could not delete the local xml file", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);
                        Console.WriteLine($"Could not delete the local xml file: {post.errorMessage}");
                        break;
                    }

                    await _log.logStepAsync(enumLogJobStep.IMPORT_XML, "Save data into DB", inject.errorMessage!, inject.success, read.returnVal!.Tables["Item"]!.Rows.Count, stepStart, stepEnd, "", (int)enumSeverity.MID, Globals.isForTest, fileInfo.returnVal.nextFileName!);

                    Console.WriteLine($"SUCCESSFULLY IMPORTED FILE {fileInfo.returnVal.nextFileName}");


                } while (runNow);

            }
            Console.WriteLine($"PROGRAM END: {DateTime.Now}");



        }







    }
}
