using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.Models
{
    public class TypeOfCrime
    {

        public int TypeOfCrimeId { get; set; }

        [Required(ErrorMessage = "Skriv inn type lovbrudd")]
        [Display(Name = "Type lovbrudd")]
        [StringLength(50, ErrorMessage = "Maks 50 tegn")]
        public string CrimeTypeName { get; set; }

        public virtual ICollection<Crime> Crimes { get; set; }
    }
}