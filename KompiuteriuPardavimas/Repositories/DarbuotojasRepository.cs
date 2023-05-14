using KompiuteriuPardavimas.Models;

namespace KompiuteriuPardavimas.Repositories
{
	/// <summary>
	/// Database operations related to 'Darbuotojas' entity
	/// </summary>
	public class DarbuotojasRepository
	{
		public static List<DarbuotojasL> List()
		{
			var query = 
				$@"SELECT 
					darb.kodas,
					darb.vardas,
					darb.pavarde,
					biur.id_BIURAS
				FROM 
					`{Config.TblPrefix}darbuotojai` darb
					LEFT JOIN `{Config.TblPrefix}biurai` biur ON darb.fk_BIURAS=biur.id_BIURAS
				ORDER BY
					kodas ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<DarbuotojasL>(drc, (dre, t) =>
				{
					t.Kodas = dre.From<int>("kodas");
					t.Vardas = dre.From<string>("vardas");
					t.Pavarde = dre.From<string>("pavarde");
					t.FkBiuras = dre.From<int>("id_BIURAS");
				});
			return result;
		}

		public static DarbuotojasCE Find(int id)
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}darbuotojai` WHERE kodas=?id";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?id", id);
				});

            if (drc.Count > 0)
            {
				var result =
					Sql.MapOne<DarbuotojasCE>(drc, (dre, t) =>
					{
						// shortcut
						var auto = t.Darbuotojas;

						auto.Kodas = dre.From<int>("kodas");
						auto.Vardas = dre.From<string>("vardas");
						auto.Pavarde = dre.From<string>("pavarde");
						auto.FkBiuras = dre.From<int>("fk_BIURAS");
					});

				return result;
            }

			return null;
        }

		public static void Update(DarbuotojasCE darbuotojas)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}darbuotojai`
				SET
					vardas=?vardas,
					pavarde=?pavarde
				WHERE
					kodas=?kodas";

			Sql.Update(query, args =>
			{
				// shorcut
				var auto = darbuotojas.Darbuotojas;

				args.Add("?vardas", auto.Vardas);
				args.Add("?pavarde", auto.Pavarde);
				args.Add("?kodas", auto.Kodas);
			});
		}

		public static void Insert(DarbuotojasCE darbuotojas)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}darbuotojai`
				(
					kodas,
					vardas,
					pavarde,
					fk_BIURAS
				)
				VALUES
				(
					?kodas,
					?vardas,
					?pavarde,
					?fk_BIURAS
				)";

			Sql.Insert(query, args =>
			{
				// shortcut
				var auto = darbuotojas.Darbuotojas;

				args.Add("?kodas", auto.Kodas);
				args.Add("?vardas", auto.Vardas);
				args.Add("?pavarde", auto.Pavarde);
				args.Add("?fk_BIURAS", auto.FkBiuras);
			});
		}

		public static void Delete(int id)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}darbuotojai` WHERE kodas=?id";
			Sql.Delete(query, args =>
			{
				args.Add("?id", id);
			});
		}

		public static List<Biuras> ListBiurai()
		{
			var query =
				$@"SELECT
					*
				FROM
					`{Config.TblPrefix}biurai`
				ORDER BY
					id_BIURAS ASC";

			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<Biuras>(drc, (dre, t) =>
				{
					t.Pavadinimas = dre.From<string>("pavadinimas");
					t.Adresas = dre.From<string>("adresas");
					t.Telefonas = dre.From<string>("telefonas");
					t.ElPastas = dre.From<string>("el_pastas");
					t.ID = dre.From<int>("id_BIURAS");
				});

			return result;
		}
	}
}
