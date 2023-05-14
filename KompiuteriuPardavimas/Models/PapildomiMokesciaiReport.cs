namespace KompiuteriuPardavimas.Models.PapildomiMokesciaiReport;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// View model for a single papildomas mokestis in papildomi mokesciai report
/// </summary>
public class PapildomasMokestis
{
	[DisplayName("Suma")]
	public decimal Suma { get; set; }

	[DisplayName("Kiekis")]
	public int Kiekis { get; set; }

	[DisplayName("Mokescio pavadinimas")]
	public string MokescioPavadinimas { get; set; }

	[DisplayName("Mokescio aprasymas")]
	public string MokescioAprasymas { get; set; }
}


/// <summary>
/// View model of the whole report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }

	public List<PapildomasMokestis> PapildomiMokesciai { get; set; }

	public int VisoUzsakyta { get; set; }

	public decimal BendraSuma { get; set; }

}

