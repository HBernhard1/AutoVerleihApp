using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AutoVerleih.Models
{
    public class Verleih
    {
        [Key]
        [ReadOnly(true)]
        public int ID { get; set; }
        public int KundenId { get; set; }
        public int AutoId { get; set; }
        public DateTime DT_Von { get; set; }
        public DateTime? DT_Bis { get; set; }
        public DateTime? DT_Rueckgabe { get; set; }
        public int KM_gefahren { get; set; }
    }
}
