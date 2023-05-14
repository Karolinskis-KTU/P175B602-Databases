namespace KompiuteriuPardavimas.Models.UzsakymasReport;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// View model for single 'uzsakymas' in a report
/// </summary>
public class Uzsakymas
{
	[DisplayName("Uzsakymas")]
	public int Nr { get; set; }

	[DisplayName("Data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime UzsakymoData { get; set; }

	public string Vardas { get; set; }

	public string Pavarde { get; set; }

	public int KlientoID { get; set; }

	[DisplayName("Sudarytų užsakymų vertė")]
	public decimal Kaina { get; set; }

	[DisplayName("Užsakytų papildomų mokesčių vertė")]
	public decimal? PapildomiMokesciaiKaina { get; set; }

	public decimal BendraSuma { get; set; }

	public decimal BendraSumaPapMok { get; set; }
}

/// <summary>
/// View model for the whole report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }

	public List<Uzsakymas> Uzsakymai { get; set; }

	public decimal VisoSumaUzsakymu { get; set; }

	public decimal? VisoSumaPapMok { get; set; }
}
