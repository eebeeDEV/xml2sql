using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.CsVModels
{
    internal class CsvFuncRec
    {
		[Index(0)]
		public int? cfctfag { get; set; }
		[Index(1)]
		public string? Col02 { get; set; }
		[Index(2)]
		public string? lfctf { get; set; }
		[Index(3)]
		public string? Col04 { get; set; }
		[Index(4)]
		public string? Col05 { get; set; }
		[Index(5)]
		public string? Col06 { get; set; }
		[Index(6)]
		public DateTime? Col07 { get; set; }
	}
}
