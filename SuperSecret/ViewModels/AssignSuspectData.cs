using SuperSecret.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperSecret.ViewModels
{
    public class AssignSuspectData
    {
        public int SuspectId { get; set; }
        public int? CountryId { get; set; }

        [Display(Name = "Navn")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        [Display(Name = "Alder")]
        public int Age { get; set; }
        public bool Assigned { get; set; }

        public virtual Country Country { get; set; }

    }
}