using KompiuteriuPardavimas.Models;
using Newtonsoft.Json;
using static KompiuteriuPardavimas.Models.UzsakymasCE;

namespace KompiuteriuPardavimas.Repositories
{
	public class UzsakymasRepository
	{
		public static List<UzsakymasL> ListUzsakymas()
		{
			var query =
				$@"SELECT
					uzs.uz_nr,
					uzs.uzsakymo_data as data,
					CONCAT(darb.vardas, ' ', darb.pavarde) as darbuotojas,
					CONCAT(klie.vardas, ' ', klie.pavarde) as klientas
				FROM
					`{Config.TblPrefix}uzsakymai` uzs
					LEFT JOIN `{Config.TblPrefix}darbuotojai` darb ON uzs.fk_DARBUOTOJAS=darb.kodas
					LEFT JOIN `{Config.TblPrefix}klientai` klie ON uzs.fk_pirkejas=klie.klient_d
				ORDER BY
					uzs.uzsakymo_data DESC";

			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<UzsakymasL>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("uz_nr");
					t.UzsakymoData = dre.From<DateTime>("data");
					t.Darbuotojas = dre.From<string>("darbuotojas");
					t.Pirkejas = dre.From<string>("klientas");
				});

			return result;
		}

		public static UzsakymasCE FindUzsakymas(int id)
		{
			var query =	
				$@"SELECT 
					uzs.uz_nr,
					uzs.uzsakymo_data,
					CONCAT(darb.vardas, ' ', darb.pavarde) as darbuotojas,
					CONCAT(klie.vardas, ' ', klie.pavarde) as klientas
				FROM 
					`{Config.TblPrefix}uzsakymai` uzs
					LEFT JOIN `{Config.TblPrefix}darbuotojai` darb ON uzs.fk_DARBUOTOJAS=darb.kodas
					LEFT JOIN `{Config.TblPrefix}klientai` klie ON uzs.fk_pirkejas=klie.klient_d
				WHERE 
					uz_nr=?id";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?id", id);
				});

			var result =
				Sql.MapOne<UzsakymasCE>(drc, (dre, item) =>
				{
					// shortcut
					var auto = item.Uzsakymas;

					auto.Id = dre.From<int>("uz_nr");
					auto.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
					auto.FkDarbuotojas = dre.From<string>("darbuotojas");
					auto.FkPirkejas = dre.From<string>("klientas");
				});

			return result;
		}

		public static int InsertUzsakymas(UzsakymasCE uzsakymasCE)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}uzsakymai`
				(
					uz_nr,
					uzsakymo_data,
					fk_DARBUOTOJAS,
					fk_pirkejas
				)
				VALUES (
					?uz_nr,
					?uzsakymo_data,
					?fk_DARBUOTOJAS,
					?fk_pirkejas
				)";

			var nr =
				Sql.Insert(query, args =>
				{
					// make a shortcut
					var uzs = uzsakymasCE.Uzsakymas;

					args.Add("?uz_nr", uzs.Id);
					args.Add("?uzsakymo_data", uzs.UzsakymoData);
					args.Add("?fk_DARBUOTOJAS", uzs.FkDarbuotojas);
					args.Add("?fk_pirkejas", uzs.FkPirkejas);
				});

			return (int)nr;
		}

		public static void UpdateUzsakymas(UzsakymasCE uzsakymasCE)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}uzsakymai`
				SET
					uzsakymo_data = ?uzsakymo_data,
					fk_DARBUOTOJAS = ?fk_DARBUOTOJAS,
					fk_pirkejas = ?fk_pirkejas
				WHERE
					?uz_nr";

			Sql.Update(query, args =>
			{
				// make a shortcut
				var uzs = uzsakymasCE.Uzsakymas;

				args.Add("?uzsakymo_data", uzs.UzsakymoData);
				args.Add("?fk_DARBUOTOJAS", uzs.FkDarbuotojas);
				args.Add("?fk_pirkejas", uzs.FkPirkejas);

				args.Add("?uz_nr", uzs.Id);
			});
		}

		public static void DeleteSutartis(int nr)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}uzsakymai` where uz_nr=?uz_nr";
			Sql.Delete(query, args =>
			{
				args.Add("?uz_nr", nr);
			});
		}

		internal static void InsertPapildomasMokestis(int id, UzsakymasCE.UzsakytasPapildomasMokestisM upPapMok)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}papildomi_mokesciai`
				(
					kaina,
					kiekis,
					fk_UZSAKYMAS,
					fk_MOKESTIS
				)
				VALUES
				(
					?kaina,
					?kiekis,
					?fk_UZSAKYMAS,
					?fk_MOKESTIS
				)";

			Sql.Insert(query, args =>
			{
				args.Add("?kaina", upPapMok.Kaina);
				args.Add("?kiekis", upPapMok.Kiekis);
				args.Add("?fk_UZSAKYMAS", upPapMok.IDUzsakymas);
				args.Add("?fk_MOKESTIS", upPapMok.IDMokestis);
			});
		}

		internal static void UpdateKompiuteris(int uzskID, string kompID)
		{
			var KompID = Int32.Parse(kompID);

			var query =
				$@"UPDATE `{Config.TblPrefix}kompiuteriai`
				SET
					fk_UZSAKYMAS = ?fk_UZSAKYMAS
				WHERE
					komp_id=?komp_id";
			Sql.Update(query, args =>
			{
				args.Add("?fk_UZSAKYMAS", uzskID);
				args.Add("?komp_id", KompID);
			});
		}

		public static void DeleteUzsakytiKompiuteriaiForUzsakymas(int id)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}kompiuteriai`
				SET
					fk_UZSAKYMAS = NULL
				WHERE
					fk_UZSAKYMAS = ?id";

			Sql.Update(query, args =>
			{
				args.Add("?id", id);
			});
		}

		public static void DeletePapildomiMokesciaiForUzsakymas(int uzsakymasID)
		{
			var query =
				$@"DELETE FROM papild
				USING `{Config.TblPrefix}papildomi_mokesciai` as papild
				WHERE papild.fk_UZSAKYMAS=?uzsakymasID";

			Sql.Delete(query, args =>
			{
				args.Add("?uzsakymasID", uzsakymasID);
			});
		}

		public static IList<UzsakymasCE.UzsakytasKompiuterisM> ListUzsakytiKompiuteriai(int uzsakymasID)
		{
			var query =
				$@"SELECT 
					*
				FROM
					`{Config.TblPrefix}kompiuteriai`
				WHERE
					fk_UZSAKYMAS=?uzsakymasID
				ORDER BY
					pavadinimas ASC,
					procesorius ASC";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?uzsakymasID", uzsakymasID);
				});

			var result =
				Sql.MapAll<UzsakymasCE.UzsakytasKompiuterisM>(drc, (dre, t) =>
				{
					t.Pavadinimas = dre.From<string>("pavadinimas");
				});

			for (int i = 0; i < result.Count; i++)
			{
				result[i].inListID = i;
			}

			return result;
		}

		internal static IList<UzsakytasPapildomasMokestisM> ListPapildomiMokesciai()
		{
			var query =
				$@"SELECT
					*
				FROM
					`{Config.TblPrefix}mokesciai` mok
				ORDER BY
					id_MOKESTIS ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<UzsakytasPapildomasMokestisM>(drc, (dre, t) =>
				{
					t.Pavadinimas = dre.From<string>("pavadinimas");
					t.Aprasymas = dre.From<string>("aprasymas");
					t.IDMokestis = dre.From<int>("id_MOKESTIS");
				});

			return result;
		}

		internal static IList<UzsakytasPapildomasMokestisM> ListPapildomiMokesciai(int uzsakymasID)
		{
			var query =
				$@"SELECT *
				FROM
					`{Config.TblPrefix}mokesciai` mok
					LEFT JOIN `{Config.TblPrefix}papildomi_mokesciai` papild ON mok.id_MOKESTIS=papild.fk_MOKESTIS
				WHERE
					papild.fk_UZSAKYMAS=?uzsakymasID
				ORDER BY
					mok.pavadinimas ASC";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?uzsakymasID", uzsakymasID);
				});

			var result =
				Sql.MapAll<UzsakytasPapildomasMokestisM>(drc, (dre, t) =>
				{
					t.Pavadinimas = dre.From<string>("pavadinimas");
					t.Aprasymas = dre.From<string>("aprasymas");
					t.IDMokestis = dre.From<int>("id_MOKESTIS");
					t.Kaina = dre.From<int>("kaina");
					t.Kiekis = dre.From<int>("kiekis");
					t.IDUzsakymas = dre.From<int>("fk_UZSAKYMAS");
				});

			for (int i = 0; i < result.Count; i++)
			{
				result[i].inListID = i;
			}

			return result;
		}
	}
}
