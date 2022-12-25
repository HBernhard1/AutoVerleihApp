using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using AutoVerleih.Filter;
using static System.Net.Mime.MediaTypeNames;

namespace AutoVerleih.Models
{
    [Table("Kunden")]
    public partial class Kunden
    {
    
        public int KundenId { get; set; }

        public string? Name { get; set; }

        public string? Plz { get; set; }

        public string? Ort { get; set; }

        public string? Strasse { get; set; }

        public DateTime? DT_AnAend { get; set; } = DateTime.Now;

  //      public virtual ICollection<Verleih> Verleihs{ get; set; }
    }
}