using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Interfaces
{
    public interface IXml2SqlController
    {
        void copyXsdFiles();
        Task injectXml();
        Task sendMessage();
        Task sendTeamsLastImportDates();
        Task sendTeamsLastKpiDateValues();
    }
}
