using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers.Responses
{
    public class RetVal
    {
        public bool success { get; set; }
        public string? errorMessage { get; set; }
        public int? returnInt { get; set; }

    }

    public class RetVal<T> : RetVal
    {
        public T? returnVal { get; set; }
    }

    public class RetVal<T, U> : RetVal
    {
        public T? returnVal_T { get; set; }
        public U? returnVal_U { get; set; }
    }
}
