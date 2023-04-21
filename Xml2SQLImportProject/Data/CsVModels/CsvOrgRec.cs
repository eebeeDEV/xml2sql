using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.CsVModels
{

    public class CsvOrgRec
    {
        [Index(0)]
        public string? col00 { get; set; }
        [Index(1)]
        public string? col01 { get; set; }
        [Index(2)]
        public int? uidu { get; set; }
        [Index(3)]
        public string? col03 { get; set; }
        [Index(4)]
        public string? col04 { get; set; }
        [Index(5)]
        public int? uidSupU { get; set; }
        [Index(6)]
        public string? col06 { get; set; }
        [Index(7)]
        public string? col07 { get; set; }
        [Index(8)]
        public string? col08 { get; set; }
        [Index(9)]
        public string? col09 { get; set; }
        [Index(10)]
        public string? col10 { get; set; }
        [Index(11)] 
        public string? lunt { get; set; }
        [Index(12)] 
        public int? uidMgr { get; set; }
        [Index(13)]
        public string? col13 { get; set; }
        [Index(14)]
        public string? col14 { get; set; }
        [Index(15)]
        public string? col15 { get; set; }
        [Index(16)]
        public string? col16 { get; set; }
        [Index(17)]
        public string? col17 { get; set; }
        [Index(18)]
        public string? col18 { get; set; }
        [Index(19)]
        public string? col19 { get; set; }
        [Index(20)]
        public string? col20 { get; set; }
        [Index(21)] 
        public string? cDep { get; set; }
        [Index(22)]
        public string? col22 { get; set; }
        [Index(23)]
        public string? col23 { get; set; }
        [Index(24)]
        public string? col24 { get; set; }

    }
}