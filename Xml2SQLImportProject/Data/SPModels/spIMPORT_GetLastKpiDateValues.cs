using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.SPModels
{
    public class spIMPORT_GetLastKpiDateValues
    {
        public int? groupId { get; set; }
        public string? fileName { get; set; }
        public string? status { get; set; }
        public string? currentDate { get; set; }
        public string? today { get; set; }
        [Column("1dayAgo")]
        public string? _1dayAgo { get; set; }
        [Column("2daysAgo")]
        public string? _2daysAgo { get; set; }
        [Column("3daysAgo")]
        public string? _3daysAgo { get; set; }
        [Column("4daysAgo")]
        public string? _4daysAgo { get; set; }
        [Column("5daysAgo")]
        public string? _5daysAgo { get; set; }
        [Column("6daysAgo")]
        public string? _6daysAgo { get; set; }
        [Column("7daysAgo")]
        public string? _7daysAgo { get; set; }
        [Column("8daysAgo")]
        public string? _8daysAgo { get; set; }
        [Column("9daysAgo")]
        public string? _9daysAgo { get; set; }       						

    }

    public class spIMPORT_GetLast2KpiDateValues
    {
        public bool? isDifferent { get; set; }
        public string? fileName { get; set; }
        public string? status { get; set; }
        public string? currentDate { get; set; }
        public int? today { get; set; }        
        public int? yesterday { get; set; }      

    }


}
