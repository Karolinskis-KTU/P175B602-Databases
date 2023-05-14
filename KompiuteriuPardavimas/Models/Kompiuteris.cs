using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KompiuteriuPardavimas.Models
{
	/// <summary>
	/// 'Kompiuteris' in list form
	/// </summary>
	public class KompiuterisL
	{
		[DisplayName("Id")]
		public int Id { get; set; }

		[DisplayName("Pavadinimas")]
		public string Pavadinimas { get; set; }

		[DisplayName("Procesorius")]
		public string Procesorius { get; set; }

		[DisplayName("Vaizdo Plokštė")]
		public string VaizdoPlokste { get; set; }

		[Required]
		[DisplayName("Maitinimo Blokas")]
		public string MaitinimoBlokas { get; set; }

		[Required]
		[DisplayName("Foreign Key TIEKEJAS")]
		public string FkTiekejas { get; set; }

		[Required]
		[DisplayName("Foreign Key UZSAKYMAS")]
		public string FkUzsakymas { get; set; }

		[Required]
		[DisplayName("Motinines Plokstes Tipas")]
		public string MotininesPlokstesTipas { get; set; }

		[Required]
		[DisplayName("Korpusas")]
		public string Korpusas { get; set; }

		[Required]
		[DisplayName("Ausinimas")]
		public string Ausinimas { get; set; }

		//[DisplayName("Kietasis Diskas")]
		//public string KietasisDiskas { get; set; }
	}

    public class KompiuterisCE
    {
        /// <summary>
        /// Kompiuteris
        /// </summary>
        public class KompiuterisM
        {
			[DisplayName("Id")]
			public int Id { get; set; }

			[Required]
			[DisplayName("Pavadinimas")]
			public string Pavadinimas { get; set; }

			[Required]
			[DisplayName("Procesorius")]
			public string Procesorius { get; set; }

			[Required]
			[DisplayName("Pagrindine atmintis")]
			public string PagrindineAtmintis { get; set; }

			[Required]
			[DisplayName("Vaizdo Plokste")]
			public string VaizdoPlokste { get; set; }

			[Required]
			[DisplayName("Maitinimo Blokas")]
			public string MaitinimoBlokas { get; set; }

			[Required]
			[DisplayName("Foreign Key TIEKEJAS")]
			public int FkTiekejas { get; set; }

			[DisplayName("Foreign Key UZSAKYMAS")]
			public string FkUzsakymas { get; set; }


			[Required]
			[DisplayName("Motinines Plokstes Tipas")]
			public int MotininesPlokstesTipas { get; set; }

			[Required]
			[DisplayName("Korpusas")]
			public int Korpusas { get; set; }

			[Required]
			[DisplayName("Ausinimas")]
			public int Ausinimas { get; set; }
		}

		public class PriklausantysDiskaiM
		{
			/// <summary>
			/// ID of the record in the form. Is used when adding and removing records
			/// </summary>
			public int inListID { get; set; }

			[Required]
			[DisplayName("Gamintojas")]
			public string Gamintojas { get; set; }

			[Required]
			[DisplayName("Talpa")]
			public int Talpa { get; set; }

			[Required]
			[DisplayName("Tipas")]
			public string Tipas { get; set; }
		}

		public class ListsM
		{
			public IList<SelectListItem> Ausinimai { get; set; }
			public IList<SelectListItem> Motinines { get; set; }
			public IList<SelectListItem> Korpusai { get; set; }
			public IList<SelectListItem> Atmintys { get; set; }
			public IList<SelectListItem> Tiekejai { get; set; }
		}

		/// <summary>
		/// Kompiuteris
		/// </summary>
		public KompiuterisM Kompiuteris { get; set; } = new KompiuterisM();

		public IList<PriklausantysDiskaiM> KompiuterioKietiejiDiskai { get; set; } = new List<PriklausantysDiskaiM>();

		/// <summary>
		/// Lists for drop down controls
		/// </summary>
		public ListsM Lists { get; set; } = new ListsM();
	}

	/// <summary>
	/// 'Ausinimai' enumerator in lists
	/// </summary>
	public class AusinimuTipai
	{
		public int Id { get; set; }
		public string Pavadinimas { get; set; }
	}

	/// <summary>
	/// 'MotininiuTipai' enumerator in lists
	/// </summary>
	public class MotininiuTipai
    {
        public int Id { get; set; }
        public string Pavadinimas { get; set; }
    }

    /// <summary>
    /// 'KorpusuTipai' enumerator in lists
    /// </summary>
    public class KorpusuTipai
    {
        public int Id { get; set; }
        public string Pavadinimas { get; set; }
    }

	/// <summary>
	/// AtmintiesTipai' enumerator in lists
	/// </summary>
	public class AtmintiesTipai
	{
		public int Id { get; set; }
		public string Pavadinimas { get; set; }
	}

	public class Tiekejai
	{
		public int Id { get; set; }
		public string Pavadinimas { get; set; }
	}
}
