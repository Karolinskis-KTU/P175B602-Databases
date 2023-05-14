//using DataAnnotationsExtensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KompiuteriuPardavimas.Models
{
    public class Tiekejas
    {
        [Required]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Pavadinimas")]
        public string Pavadinimas { get; set; }

        [Required]
        [DisplayName("Telefonas")]
        public string Telefonas { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Elektroninis pastas")]
        public string ElPastas { get; set; }

        [Required]
        [DisplayName("Adresas")]
        public string Adresas { get; set; }
    }
}
