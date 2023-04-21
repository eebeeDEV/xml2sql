using Xml2SqlImport.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers.Responses
{
    public class TaskResult
    {
        public bool success { get; set; }
        public string? error { get; set; }
        public string? errorType { get; set; }
        public int? resultInt { get; set; }
        public enumLogJobStep logJobType { get; set; }
        public string? logTableName { get; set; }
        public string? logStepDescr { get; set; }
    }

    public class TaskResult<T>: TaskResult
    {
        public IEnumerable<T>? returnValue { get; set; }      
    }


}
