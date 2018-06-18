using SuperSecret.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.ViewModels
{
    public class AssignCrimeData
    {
        public int CrimeId { get; set; }
        public int TypeOfCrimeId { get; set; }

        [Display(Name = "Beskrivelse")]
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public bool Assigned { get; set; }

        public virtual TypeOfCrime TypeOfCrime { get; set; }

    }
}