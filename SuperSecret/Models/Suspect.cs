using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.Models
{
    public class Suspect
    {
        public int SuspectId { get; set; }
        public int? CountryId { get; set; }
        public int? PictureId { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Skriv inn navn")]
        [StringLength(50, ErrorMessage = "Maks 50 tegn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Skriv inn alias")]
        [StringLength(50, ErrorMessage = "Maks 50 tegn")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Legg til alder")]
        [Display(Name = "Alder")]
        public int Age { get; set; }

        public virtual ICollection<Crime> Crimes { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<FilePath> FilePaths { get; set; }
    }
}