using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KompiuteriuPardavimas.Models
{
    public class Klientas
    {
        [Required]
        [DisplayName("Id")]
        public string Id { get; set; }

        [Required]
        [DisplayName("Vardas")]
        public string Vardas { get; set; }

        [Required]
        [DisplayName("Pavarde")]
        public string Pavarde { get; set; }

        [Required]
        [DisplayName("Telefono numeris")]
        public string Telefonas { get; set; }

        [Required]
        [DisplayName("Elektroninis pastas")]
        public string ElPastas { get; set; }

        [Required]
        [DisplayName("Adresas")]
        public string Adresas { get; set; }
    }
}
