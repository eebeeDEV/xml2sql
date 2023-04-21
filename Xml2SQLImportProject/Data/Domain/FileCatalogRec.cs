using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Data.Domain
{

    [Table("FileCatalog") ]
    public class FileCatalogRec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? fileId { get; set; }              
        public string? fileBaseName { get; set; }
        public string? fileKpiTypeClass { get; set; }
        public string? fileKpiItemTable { get; set; }
        public DateTime? fileAdded { get; set; }
        public DateTime? fileLastImportDate { get; set; }
        public DateTime? fileLastImportDateTime { get; set; }



    }

}
