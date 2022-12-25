using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoVerleih.Models
{
    public class Verleih
    {
        [Key]
        [ReadOnly(true)]
        public int ID { get; set; }
        [Required(ErrorMessage = "KundenId fehlt")]
        public int KundenId { get; set; }
        [Required(ErrorMessage = "AutoId fehlt")]
        public int AutoId { get; set; }
        [Required(ErrorMessage = "Ausleihdatum fehlt")]
        public DateTime DT_Von { get; set; }
        public DateTime? DT_Bis { get; set; }
        public DateTime? DT_Rueckgabe { get; set; }
        public int KM_gefahren { get; set; }

        [NotMapped]
        public List<SelectListItem> SelectAutos { get; set; }
    }
}
