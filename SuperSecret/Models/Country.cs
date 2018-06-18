using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Skriv inn land")]
        [Display(Name = "Land")]
        [StringLength(255)]
        public string CountryName { get; set; }

        public virtual ICollection<Suspect> Suspects { get; set; }
    }
}