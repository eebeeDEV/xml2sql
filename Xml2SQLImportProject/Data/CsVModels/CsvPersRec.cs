using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.CsVModels
{


    public class CsvPersRec
    {
		[Index(0)]
		public int? nMateFox { get; set; }
		[Index(1)]
		public int? uidP { get; set; }
		[Index(2)] 
		public string? lNom { get; set; }
		[Index(3)] 
		public string? lPrn { get; set; }
		[Index(4)] 
		public string? lNomUsl { get; set; }
		[Index(5)] 
		public string? lPrnUsl { get; set; }
		[Index(6)] 
		public DateTime? dDob { get; set; }
		[Index(7)]
		public int? cTitle { get; set; }
		[Index(8)] 
		public int? cSex { get; set; }
		[Index(9)] 
		public int? cFctFag { get; set; }
		[Index(10)] 
		public int? cLan { get; set; }
		[Index(11)] 
		public DateTime? dIn { get; set; }
		[Index(12)] 
		public DateTime? dout { get; set; }
		[Index(13)] 
		public string? kUser { get; set; }
		[Index(14)] 
		public string? lPho { get; set; }
		[Index(15)] 
		public DateTime? dUidUi { get; set; }
		[Index(16)] 
		public DateTime? dUidUo { get; set; }
		[Index(17)] 
		public int? uidU { get; set; }
		[Index(18)] 
		public int? typeEmp { get; set; } //1 employés actifs - 6 expatriés - 4 externes - 9 sortis
		[Index(19)]
		public string? Col20 { get; set; }
		[Index(20)]
		public string? Col21 { get; set; }
		[Index(21)]
		public string? Col22 { get; set; }
		[Index(22)]
		public string? lEml { get; set; }
		[Index(23)]
		public string? Col24 { get; set; }
		[Index(24)]
		public string? lIntPho { get; set; }
		[Index(25)]
		public string? Col26 { get; set; }
		[Index(26)]
		public string? Col27 { get; set; }
		[Index(27)]
		public string? lImp { get; set; }
		[Index(28)]
		public string? lsit { get; set; }
		[Index(29)]
		public string? Col30 { get; set; }
		[Index(30)]
		public string? lGsm { get; set; }
		[Index(31)]
		public string? Col32 { get; set; }
		[Index(32)]
		public string? Col33 { get; set; }
		[Index(33)]
		public DateTime? dInaIn { get; set; }
		[Index(34)]
		public DateTime? dInaOut { get; set; }
		[Index(35)]
		public string? Col36 { get; set; }
		[Index(36)]
		public string? Col37 { get; set; }
		[Index(37)]
		public string? Col38 { get; set; }
		
	}
}
