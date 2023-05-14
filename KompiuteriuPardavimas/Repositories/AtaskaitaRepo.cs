namespace KompiuteriuPardavimas.Repositories;

using MySql.Data.MySqlClient;

using UzsakymaiReport = KompiuteriuPardavimas.Models.UzsakymasReport;
using PapildomiMokesciaiReport = KompiuteriuPardavimas.Models.PapildomiMokesciaiReport;

public class AtaskaitaRepo
{
	public static List<UzsakymaiReport.Uzsakymas> GetUzsakymas(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				uzs.uz_nr AS Nr,
				uzs.uzsakymo_data AS UzsakymoData,
				klie.vardas AS Vardas,
				klie.pavarde AS Pavarde,
				klie.klient_d AS KlientoID,
				sask.suma AS Kaina,
				SUM(papMok.kiekis*papMok.kaina) AS PapildomiMokesciaiKaina,
				(
					SELECT 
						SUM(sask2.suma) 
					FROM 
						`{Config.TblPrefix}saskaitos` sask2 
					WHERE 
						sask2.fk_UZSAKYMAS 
					IN (
						SELECT 
							uzs2.uz_nr 
						FROM 
							`{Config.TblPrefix}uzsakymai` uzs2 
						WHERE 
							uzs2.fk_pirkejas = klie.klient_d
					)
				) AS BendraSuma,
				(
					SELECT 
						IFNULL (SUM(papMok2.kaina*papMok2.kiekis), 0) 
					FROM 
						`{Config.TblPrefix}papildomi_mokesciai` papMok2 
					WHERE 
						papMok2.fk_UZSAKYMAS 
					IN (
						SELECT 
							uzs2.uz_nr 
						FROM 
							`{Config.TblPrefix}uzsakymai` uzs2 
						WHERE 
							uzs2.fk_pirkejas = klie.klient_d
					)
				) AS BendraSumaPapMok
				FROM
					`{Config.TblPrefix}uzsakymai` uzs
					LEFT JOIN `{Config.TblPrefix}klientai` klie ON klie.klient_d=uzs.fk_pirkejas
					INNER JOIN `{Config.TblPrefix}saskaitos` sask ON sask.fk_UZSAKYMAS = uzs.uz_nr
					LEFT JOIN `{Config.TblPrefix}papildomi_mokesciai` papMok ON papMok.fk_UZSAKYMAS=uzs.uz_nr
				WHERE
					uzs.uzsakymo_data >= IFNULL(?nuo, uzs.uzsakymo_data)
					AND uzs.uzsakymo_data <= IFNULL(?iki, uzs.uzsakymo_data)
				GROUP BY
					Nr, KlientoID
				ORDER BY
					Pavarde;";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result =
			Sql.MapAll<UzsakymaiReport.Uzsakymas>(drc, (dre, t) => {
				t.Nr = dre.From<int>("Nr");
				t.UzsakymoData = dre.From<DateTime>("UzsakymoData");
				t.Vardas = dre.From<string>("Vardas");
				t.Pavarde = dre.From<string>("Pavarde");
				t.KlientoID = dre.From<int>("KlientoID");
				t.Kaina = dre.From<decimal>("Kaina");
				t.PapildomiMokesciaiKaina = dre.From<decimal?>("PapildomiMokesciaiKaina");
				t.BendraSumaPapMok = dre.From<decimal>("BendraSumaPapMok");
				t.BendraSuma = dre.From<decimal>("BendraSuma");
			});

		return result;
	}

	public static List<PapildomiMokesciaiReport.PapildomasMokestis> GetPapildomiMokesciaiOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				SUM(papMok.kiekis) AS kiekis,
				SUM(papMok.kiekis*papMok.kaina) AS suma,
				mok.pavadinimas,
				mok.aprasymas
			FROM
				`{Config.TblPrefix}mokesciai` mok,
				`{Config.TblPrefix}papildomi_mokesciai` papMok,
				`{Config.TblPrefix}uzsakymai` uzs
			WHERE
				mok.id_MOKESTIS = papMok.fk_MOKESTIS
				AND papMok.fk_UZSAKYMAS = uzs.uz_nr
				AND uzs.uzsakymo_data >= IFNULL(?nuo, uzs.uzsakymo_data)
				AND uzs.uzsakymo_data <= IFNULL(?iki, uzs.uzsakymo_data)
			GROUP BY
				mok.id_MOKESTIS, mok.pavadinimas
			ORDER BY
				suma DESC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result =
			Sql.MapAll<PapildomiMokesciaiReport.PapildomasMokestis>(drc, (dre, t) => {
				t.Suma = dre.From<decimal>("suma");
				t.Kiekis = dre.From<int>("kiekis");
				t.MokescioPavadinimas = dre.From<string>("pavadinimas");
				t.MokescioAprasymas = dre.From<string>("aprasymas");
			});

		return result;
	}

	public static PapildomiMokesciaiReport.Report GetTotalPapildomiMokesciaiOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				SUM(papMok.kiekis) AS kiekis,
				SUM(papMok.kiekis*papMok.kaina) AS suma
			FROM
				`{Config.TblPrefix}mokesciai` mok,
				`{Config.TblPrefix}papildomi_mokesciai` papMok,
				`{Config.TblPrefix}uzsakymai` uzs
			WHERE
				mok.id_MOKESTIS = papMok.fk_MOKESTIS
				AND papMok.fk_UZSAKYMAS = uzs.uz_nr
				AND uzs.uzsakymo_data >= IFNULL(?nuo, uzs.uzsakymo_data)
				AND uzs.uzsakymo_data <= IFNULL(?iki, uzs.uzsakymo_data)";

		var drc =
			Sql.Query(query, args =>
			{
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result =
			Sql.MapOne<PapildomiMokesciaiReport.Report>(drc, (dre, t) => {
				t.VisoUzsakyta = dre.From<int?>("kiekis") ?? 0;
				t.BendraSuma = dre.From<decimal?>("suma") ?? 0;
			});

		return result;
	}

}
