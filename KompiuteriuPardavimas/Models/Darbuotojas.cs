using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KompiuteriuPardavimas.Models
{
	public class DarbuotojasL
	{
		[Required]
		[DisplayName("Tabelio Nr.")]
		public int Kodas { get; set; }

		[Required]
		[DisplayName("Vardas")]
		public string Vardas { get; set; }

		[Required]
		[DisplayName("Pavarde")]
		public string Pavarde { get; set; }

		[Required]
		[DisplayName("Foreign Key Biuras")]
		public int FkBiuras { get; set; }
	}

	public class DarbuotojasCE
    {
        public class DarbuotojasM
        {
			[Required]
			[DisplayName("Tabelio Nr.")]
			public int Kodas { get; set; }

			[Required]
			[DisplayName("Vardas")]
			public string Vardas { get; set; }

			[Required]
			[DisplayName("Pavarde")]
			public string Pavarde { get; set; }

			[Required]
			[DisplayName("Foreign Key Biuras")]
			public int FkBiuras { get; set; }
		}

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Biurai { get; set; }
		}

		/// <summary>
		/// Lists for drop down controls
		/// </summary>
		public ListsM Lists { get; set; } = new ListsM();

		/// <summary>
		/// Darbuotojas
		/// </summary>
		public DarbuotojasM Darbuotojas { get; set; } = new DarbuotojasM();
	}


	public class Biuras
	{
		public string Pavadinimas { get; set; }
		public string Adresas { get; set; }
		public string Telefonas { get; set; }
		public string ElPastas { get; set; }
		public int ID { get; set; }
	}
}
