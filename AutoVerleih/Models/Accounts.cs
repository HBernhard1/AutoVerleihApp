using System.ComponentModel.DataAnnotations;

namespace AutoVerleih.Models
{
    public class Accounts
    {
        [Key]
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

    }
}
