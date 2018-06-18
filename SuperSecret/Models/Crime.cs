using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.Models
{
    public class Crime
    {
        public int CrimeId { get; set; }
        public int TypeOfCrimeId { get; set; }

        [Required(ErrorMessage = "Legg til en beskrivelse")]
        [Display(Name = "Beskrivelse")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Legg inn dato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual ICollection<Suspect> Suspects { get; set; }
        public virtual TypeOfCrime TypeOfCrime { get; set; }
    }
}