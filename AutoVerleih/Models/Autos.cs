using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoVerleih.Models
{
    [Table("Autos")]
    public class Autos
    {
        [Key]
        [ReadOnly(true)]
        public int AutoId { get; set; }
        public string Marke { get; set; }
        public string Type { get; set; }
        public string Farbe { get; set; }
        public decimal MietPreis_HH { get; set; }
        public decimal MietPreis_TG { get; set; }
        public decimal MietPreis_WE { get; set; }
        public bool vermietet { get; set; }
        public string Bild { get; set; }


        [NotMapped]
        public int KM_gesamt { get; set; }

        [NotMapped]
        public string AuswahlListe { 
            get
            {
                return string.Format("Id: {0} / Farbe: {1} / Type: {2}", this.AutoId, this.Farbe, this.Type);
            }
            
        }

    }
}
