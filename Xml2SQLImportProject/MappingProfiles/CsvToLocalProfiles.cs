using AutoMapper;
using Xml2SqlImport.Data.CsVModels;
using Xml2SqlImport.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.MappingProfiles
{
    internal class CsvToLocalProfiles: Profile
    {
        public CsvToLocalProfiles()
        {
            AllowNullDestinationValues = true;

            // hierarchy
            //CreateMap<CsvOrgRec, StagingOrgRec>();
            //CreateMap<CsvPersRec, StagingPersRec>();
            //CreateMap<CsvFuncRec, FileCatalogRec>();

        }
    }
}
