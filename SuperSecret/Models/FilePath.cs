using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.Models
{
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }

        public FileType Filetype { get; set; }
        public int SuspectId { get; set; }
        public virtual Suspect Suspect { get; set; }
    }
}