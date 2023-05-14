namespace KompiuteriuPardavimas.Controllers;

using KompiuteriuPardavimas.Repositories;
using Microsoft.AspNetCore.Mvc;


using UzsakymaiReport = KompiuteriuPardavimas.Models.UzsakymasReport;
using PapildomiMokesciaiReport = KompiuteriuPardavimas.Models.PapildomiMokesciaiReport;


/// <summary>
/// Controller for producing reports.
/// </summary>
public class ReportsController : Controller
{
	/// <summary>
	/// Produces 'uzsakymai' report
	/// </summary>
	/// <param name="dateFrom">Starting date. Can be null</param>
	/// <param name="dateTo">Ending date. Can be null</param>
	/// <returns>Report view.</returns>
	[HttpGet]
	public ActionResult Uzsakymai(DateTime? dateFrom, DateTime? dateTo)
	{
		var report = new UzsakymaiReport.Report();
		report.DateFrom = dateFrom;
		report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); // move time of end date to end of day

		report.Uzsakymai = AtaskaitaRepo.GetUzsakymas(report.DateFrom, report.DateTo);
		report.VisoSumaPapMok = 0;


		foreach(var item in report.Uzsakymai)
		{
			report.VisoSumaUzsakymu += item.BendraSuma;
			report.VisoSumaPapMok += item.PapildomiMokesciaiKaina;
			Console.WriteLine("Vardas: {0}", report.VisoSumaPapMok);
		}

		return View(report);
	}

	/// <summary>
	/// Produces 'PapildomiMokesciai' report.
	/// </summary>
	/// <param name="dateFrom">Starting date. Can be null</param>
	/// <param name="dateTo">Ending date. Can be null</param>
	/// <returns>Report view.</returns>
	[HttpGet]
	public ActionResult PapildomiMokesciai(DateTime? dateFrom, DateTime? dateTo)
	{
		var report = AtaskaitaRepo.GetTotalPapildomiMokesciaiOrdered(dateFrom, dateTo);
		report.DateFrom = dateFrom;
		report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); // move time of end date to end of day

		report.PapildomiMokesciai = AtaskaitaRepo.GetPapildomiMokesciaiOrdered(dateFrom, dateTo);

		return View(report);
	}
}
