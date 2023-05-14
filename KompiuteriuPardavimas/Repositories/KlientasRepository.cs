using KompiuteriuPardavimas.Models;

namespace KompiuteriuPardavimas.Repositories
{
	/// <summary>
	/// Database operations releated to 'Klientas' entity
	/// </summary>
	public class KlientasRepository
	{
		public static List<Klientas> List()
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}klientai` ORDER BY klient_d ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<Klientas>(drc, (dre, t) =>
				{
					t.Id = dre.From<string>("klient_d");
					t.Vardas = dre.From<string>("vardas");
					t.Pavarde = dre.From<string>("pavarde");
					t.Telefonas = dre.From<string>("telefono_numeris");
					t.ElPastas = dre.From<string>("el_pastas");
					t.Adresas = dre.From<string>("adresas");
				});

			return result;
		}

		public static Klientas Find(string id)
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}klientai` ORDER BY klient_d=?id";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?id", id);
				});

            if (drc.Count > 0)
            {
				var result =
					Sql.MapOne<Klientas>(drc, (dre, t) =>
					{
						t.Id = dre.From<string>("klient_d");
						t.Vardas = dre.From<string>("vardas");
						t.Pavarde = dre.From<string>("pavarde");
						t.Telefonas = dre.From<string>("telefono_numeris");
						t.ElPastas = dre.From<string>("el_pastas");
						t.Adresas = dre.From<string>("adresas");
					});

				return result;
            }

			return null;
        }

		public static void Insert(Klientas klientas)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}klientai`
				(
					klient_d,
					vardas,
					pavarde,
					telefono_numeris,
					el_pastas,
					adresas
				)
				VALUES
				(
					?klient_d,
					?vardas,
					?pavarde,
					?telefono_numeris,
					?el_pastas,
					?adresas					
				)";

			Sql.Insert(query, args =>
			{
				args.Add("?klient_d", klientas.Id);
				args.Add("?vardas", klientas.Vardas);
				args.Add("?pavarde", klientas.Pavarde);
				args.Add("?telefono_numeris", klientas.Telefonas);
				args.Add("?el_pastas", klientas.ElPastas);
				args.Add("?adresas", klientas.Adresas);
			});
		}

		public static void Update(Klientas klientas)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}klientai`
				SET
					vardas=?vardas,
					pavarde=?pavarde,
					telefono_numeris=?telefono_numeris,
					el_pastas=?el_pastas,
					adresas=?adresas
				WHERE
					klient_d=?klient_d";

			Sql.Update(query, args =>
			{
				args.Add("?klient_d", klientas.Id);

				args.Add("?vardas", klientas.Vardas);
				args.Add("?pavarde", klientas.Pavarde);
				args.Add("?telefono_numeris", klientas.Telefonas);
				args.Add("?el_pastas", klientas.ElPastas);
				args.Add("?adresas", klientas.Adresas);
			});
		}

		public static void Delete(string id)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}klientai` WHERE klient_d=?id";
			Sql.Delete(query, args =>
			{
				args.Add("?id", id);
			});
		}
	}
}
