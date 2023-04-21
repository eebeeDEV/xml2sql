using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Xml2SqlImport.Data.SPModels;
using Xml2SqlImport.Helpers.Responses;
using Xml2SqlImport.Interfaces;

namespace Xml2SqlImport.Services
{
    public class FileService : IFileService
    {

        private readonly ILocalDataService _db;
        private readonly IAppSettings _settings;
        private readonly ILogService _logService;

        public FileService(ILocalDataService db, IAppSettings settings, ILogService logService)
        {
            _db = db;
            _settings = settings;
            _logService = logService;
        }



        public async Task<RetVal<LocalXmlInfo>> copyXml(string xmlFile, DateTime xmlDate)
        {


            var dir = Directory.GetCurrentDirectory();
            var ret = new RetVal<LocalXmlInfo> { success = false, errorMessage = "Could not copy the files", returnVal = new LocalXmlInfo() };

            try
            {
                //xmlFile = "//jpfile01/#FILES/DATA_INTELLIGENCE_PEGA/data/P/archive/AG_NL_Claims_Work_IncomingDoc_BIXExtract_Delta_230122.xml";

                Console.WriteLine($"Copying the file: {xmlFile}");
                xmlFile = Path.Combine(_settings.currentFile!.fileImportFolder!, xmlFile);


                var rBypass = new FileMissingOrBypass { isBypassed = false, isOk = false, isEmpty = false, isMissing = false };
                var isMissing = !File.Exists(xmlFile);

                // check if the file can be bypassed
                var bypass = await _logService.isDateToBypass(xmlDate, isMissing);


                if (isMissing)
                {
                    rBypass.isBypassed = bypass.returnVal;
                    rBypass.success = bypass.success;
                    rBypass.errorMessage = bypass.errorMessage;
                    ret.returnVal.bypassInfo = rBypass;
                    ret.errorMessage = "Xml file does not exist.";
                    ret.success = false;
                    return ret;
                }

                // build the xsd _file path
                var xsdFile = xmlFile.Replace(".xml", ".xsd");

                if (!File.Exists(xsdFile))
                {
                    ret.errorMessage = "Xsd file does not exist.";
                    return ret;
                }
                var xFile = new FileInfo(xmlFile);
                var sFile = new FileInfo(xsdFile);

                var xmlInfo = new LocalXmlInfo { localXmlFile = Path.Combine(dir, xFile.Name), localXsdFile = Path.Combine(dir, sFile.Name), localXmlFileName = xFile.Name, localXmlFileDate = xmlDate };
                ret.returnVal = xmlInfo;


                var l = await _copyAsync(xmlFile, xmlInfo.localXmlFile, true);
                if (!l.success)
                {
                    ret.success = false;
                    ret.errorMessage = l.errorMessage;
                    return ret;
                }
                var d = await _copyAsync(xsdFile, xmlInfo.localXsdFile, true);
                if (!d.success)
                {
                    ret.success = false;
                    ret.errorMessage = d.errorMessage;
                    return ret;
                }

                // check if the file is empty
                var xDoc = new XmlDocument();
                xDoc.Load(xmlInfo.localXmlFile);
                //var el = xDoc.GetElementsByTagName("Item");

                // if the file is empty
                if(xDoc.GetElementsByTagName("item").Count == 0)
                {
                    rBypass.isEmpty = true;
                    rBypass.isBypassed = bypass.returnVal;
                    rBypass.success = bypass.success;
                    rBypass.errorMessage = bypass.errorMessage;
                    ret.returnVal.bypassInfo = rBypass;
                    ret.errorMessage = "Xml file is empty.";
                    ret.success = false;
                    return ret;
                }


                // validate the XML againts the XSD
                var val = _validateSchema(xmlInfo.localXmlFile, xmlInfo.localXsdFile);
                if (!val.success)
                {
                    ret.success = false;
                    ret.errorMessage = val.errorMessage;
                    rBypass.isOk = true;
                    rBypass.success = true;
                    rBypass.errorMessage = null;
                    ret.returnVal.bypassInfo = rBypass;
                    deleteLocalXml(xFile.Name);
                } else
                {
                    rBypass.isOk = true;
                    rBypass.success = true;
                    rBypass.errorMessage = null;
                    ret.returnVal.bypassInfo = rBypass;
                    ret.success = true;
                }

                return ret;

            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;
                Console.WriteLine(e.Message);
            }
            return ret;

        }

        public async Task<RetVal<FileMissingOrBypass>> _isFileMissingOrBypass(string xmlFile, DateTime xmlDate)
        {
            xmlFile = Path.Combine(_settings.currentFile!.fileImportFolder!, xmlFile);
            var ret = new RetVal<FileMissingOrBypass> { success = false, errorMessage = "Could not check the files" };
            var rVal = new FileMissingOrBypass { isBypassed = false, isOk = false, isEmpty = false, isMissing = false };
            try
            {
                var isMissing = !File.Exists(xmlFile);
                rVal.isMissing = isMissing;

                if (!isMissing)
                {
                    var xDoc = new XmlDocument();
                    //string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                    //if (xmlFile.StartsWith(_byteOrderMarkUtf8))
                    //{
                    //    xmlFile = xmlFile.Remove(0, _byteOrderMarkUtf8.Length);
                    //}
                    xDoc.Load(xmlFile);
                    // if the doc has more than 1 childnodes, the document is valid
                    if (xDoc.ChildNodes.Count > 1)
                    {
                        rVal.isOk = true;
                        ret.success = true;
                        ret.errorMessage = null;
                        ret.returnVal = rVal;
                        return ret;
                    }
                    else
                    {
                        rVal.isEmpty = true;
                    }
                    xDoc = null;
                }

                var bypass = await _logService.isDateToBypass(xmlDate, isMissing);

                rVal.isBypassed = bypass.returnVal;
                rVal.success = bypass.success;
                rVal.errorMessage = bypass.errorMessage;
                ret.returnVal = rVal;

            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;
            }


            return ret;
        }

        public RetVal deleteLocalXml(string xmlFile)
        {
            var dir = Directory.GetCurrentDirectory();
            var ret = new RetVal { success = false, errorMessage = "Could not delete the files" };
            try
            {
                xmlFile = Path.Combine(dir, xmlFile);
                File.Delete(xmlFile);

                var xsdFile = xmlFile.Replace(".xml", ".xsd");
                xsdFile = Path.Combine(dir, xsdFile);
                File.Delete(xsdFile);

                ret.success = true;
                ret.errorMessage = "";
            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;
            }


            return ret;

        }


        public RetVal copyXsdFiles()
        {
            var ret = new RetVal { success = false, errorMessage = "Could not copy the xsd files" };
            var fld = "//jpfile01/#FILES/DATA_INTELLIGENCE_PEGA/data/P/archive/Data_Admin_Operator_ID_BIXExtract_Delta_";
            var lastXsd = $"{fld}230329.xsd";
            var mnths = new Dictionary<int, int>();
            //mnths.Add(220800, 32);
            mnths.Add(220900, 31);
            mnths.Add(221000, 32);
            mnths.Add(221100, 31);
            mnths.Add(221200, 32);
            mnths.Add(230100, 32);
            mnths.Add(230200, 29);
            mnths.Add(230300, 28);

            foreach (var mnth in mnths)
            {
                for (int i = 1; i < mnth.Value; i++)
                {
                    var nme = mnth.Key + i;
                    File.Copy(lastXsd, $"{fld}{nme}.xsd", true);
                    Console.WriteLine($"{fld}{nme}.xsd");
                }

            }

            ret.success= true;
            ret.errorMessage = null;


            return ret;

        }


        private async Task<RetVal> _copyAsync(string sourceFile, string destFile, bool overwrite)
        {
            var ret = new RetVal { success = false, errorMessage = "Could not copy the file" };
            try
            {
                
                int streamBuffer = 262144;

                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, streamBuffer, true))
                {
                    using (FileStream destinationStream = new FileStream(destFile, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None, streamBuffer, true))
                    {
                        await sourceStream.CopyToAsync(destinationStream, streamBuffer).ConfigureAwait(false);
                    }
                }

                ret.success = true;
                ret.errorMessage = "";
            }
            catch (Exception e)
            {
                ret.errorMessage = e.Message;
            }


            return ret;
        }

        private RetVal _validateSchema(string xmlPath, string xsdPath)
        {
            var ret = new RetVal { success = false, errorMessage = "Could not validate the XML file against XSD" };

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);

            xml.Schemas.Add(null, xsdPath);

            try
            {
                xml.Validate(null);
                ret.success = true;
                ret.errorMessage = null;
            }
            catch (XmlSchemaValidationException e)
            {
                ret.errorMessage = e.Message;

            }
            return ret;
        }
    }
}
