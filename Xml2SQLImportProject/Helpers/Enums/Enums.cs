using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers.Enums
{
    public enum enumLogJobStep
    {
        GET_NEXT_XML,
        COPY_XML_TO_LOCAL,
        READ_XML,
        IMPORT_XML,
        IMPORT_XML_POST,
        RECAP_VALUES_CALC,
        DELETE_XML_LOCAL
    }

    public enum enumCsvFileType
    {
        ORGANIZATION,
        PERSONS,
        FUNCTIONS
    }

    public enum enumSeverity
    {
        INFORMATIVE,
        LOW,
        MID,
        CRITICAL
    }

}
