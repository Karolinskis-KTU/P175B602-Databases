using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KompiuteriuPardavimas.Models
{
    /// <summary>
    /// Uzsakymas in list form
    /// </summary>
    public class UzsakymasL
    {
        [Required]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Uzsakymo data")]
        public DateTime UzsakymoData { get; set; }

        [Required(ErrorMessage = "Privaloma pateikti pardaveja")]
        [DisplayName("Foreign Key Darbuotojas")]
        public string Darbuotojas { get; set; }

        [Required(ErrorMessage = "Privaloma pateikti pirkeja")]
        [DisplayName("Foreign Key Pirkejas")]
        public string Pirkejas { get; set; }
    }

    /// <summary>
    /// Uzsakymas in create and edit forms
    /// </summary>
    public class UzsakymasCE
    {
        /// <summary>
        /// Entity data
        /// </summary>
        public class UzsakymasM
        {
            [Required]
            [DisplayName("Id")]
            public int Id { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            [DisplayName("Uzsakymo data")]
            public DateTime UzsakymoData { get; set; }

            [Required(ErrorMessage = "Privaloma pateikti pardaveja")]
            [DisplayName("Foreign Key Darbuotojas")]
            public string FkDarbuotojas { get; set; }

            [Required(ErrorMessage = "Privaloma pateikti pirkeja")]
            [DisplayName("Foreign Key Pirkejas")]
            public string FkPirkejas { get; set; }
        }

        public class UzsakytasKompiuterisM
        {
            /// <summary>
            /// ID of the record in the form. Is used when adding and removing records
            /// </summary>
            public int inListID { get; set; }

            [Required]
            [DisplayName("Pavadinimas")]
            public string Pavadinimas { get; set; }
        }

        public class UzsakytasPapildomasMokestisM
        {
            /// <summary>
            /// ID of the record in the form. Is used when adding and removing records
            /// </summary>
            public int inListID { get; set; }

            [Required]
            [DisplayName("Mokescio id")]
            public int IDMokestis { get; set; }

            [Required]
            [DisplayName("Uzsakymo id")]
            public int IDUzsakymas { get; set; }

            [Required]
            [DisplayName("Pavadinimas")]
            public string Pavadinimas { get; set; }

            [DisplayName("Aprasymas")]
            public string Aprasymas { get; set; }

            [Required]
            [DisplayName("Kaina")]
            public double? Kaina { get; set; }

            [Required]
            [DisplayName("Kiekis")]
            public int? Kiekis { get; set; }

        }

        /// <summary>
        /// Select lists for making drop downs for choosing values of entity fields.
        /// </summary>
        public class ListsM
        {
			public IList<SelectListItem> Kompiuteriai { get; set; }
			public IList<SelectListItem> Klientai { get; set; }
            public IList<SelectListItem> Darbuotojai { get; set; }
            public IList<SelectListItem> Mokesciai { get; set; }

        }

        /// <summary>
        /// Uzsakymas
        /// </summary>
        public UzsakymasM Uzsakymas { get; set; } = new UzsakymasM();

        public IList<UzsakytasKompiuterisM> UzsakytiKompiuteriai { get; set; } = new List<UzsakytasKompiuterisM>();

        public IList<UzsakytasPapildomasMokestisM> UzsakytiPapildomiMokesciai { get; set; } = new List<UzsakytasPapildomasMokestisM>();

        /// <summary>
        /// Lists for drop down controls.
        /// </summary>
        public ListsM Lists { get; set; } = new ListsM();
    }
}


