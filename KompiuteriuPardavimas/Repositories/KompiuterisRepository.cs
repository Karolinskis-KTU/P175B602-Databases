using KompiuteriuPardavimas.Models;

namespace KompiuteriuPardavimas.Repositories
{
	public class KompiuterisRepository
	{
		public static List<KompiuterisL> ListKompiuteris()
		{
			var query =
				$@"SELECT
					komp.komp_id,
					komp.pavadinimas,
					komp.procesorius,
					komp.vaizdo_plokste,
					komp.maitinimo_blokas,
					CONCAT(tiek.tiekej_d, ' | ', tiek.pavadinimas, ' | ', tiek.adresas) as tiekejas,
					CONCAT(uzsk.uz_nr, ' | ', uzsk.uzsakymo_data) as uzsakymas,                    
					mot.name as motininePav,
					korp.name as korpusasPav,
					ausin.name as ausinimasPav
				FROM
					`{Config.TblPrefix}kompiuteriai` komp
					LEFT JOIN `{Config.TblPrefix}tiekejai` tiek ON komp.fk_TIEKEJAS=tiek.tiekej_d
					LEFT JOIN `{Config.TblPrefix}uzsakymai` uzsk ON komp.fk_UZSAKYMAS=uzsk.uz_nr
					LEFT JOIN `{Config.TblPrefix}motininiu_tipai` mot ON komp.motinines_plokstes_tipas=mot.id_motininiu_tipai
					LEFT JOIN `{Config.TblPrefix}korpusu_tipai` korp ON komp.korpusas=korp.id_korpusu_tipai
					LEFT JOIN `{Config.TblPrefix}ausinimai` ausin ON komp.ausinimas=ausin.id_ausinimai
				ORDER BY
					komp.komp_id";
				// TODO: Galetu dar buti kietasis diskas

			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<KompiuterisL>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("komp_id");
					t.Pavadinimas = dre.From<string>("pavadinimas");
					t.Procesorius = dre.From<string>("procesorius");
					t.VaizdoPlokste = dre.From<string>("vaizdo_plokste");
					t.MaitinimoBlokas = dre.From<string>("maitinimo_blokas");
					t.FkTiekejas = dre.From<string>("tiekejas");
					t.FkUzsakymas = dre.From<string>("uzsakymas");
					t.MotininesPlokstesTipas = dre.From<string>("motininePav");
					t.Korpusas = dre.From<string>("korpusasPav");
					t.Ausinimas = dre.From<string>("ausinimasPav");
				});

			return result;
		}

		public static List<KompiuterisL> ListFreeKompiuteris()
		{
			var query =
				$@"SELECT
					komp.komp_id,
					komp.pavadinimas,
					komp.procesorius,
					komp.vaizdo_plokste,
					komp.maitinimo_blokas,
					CONCAT(tiek.tiekej_d, ' | ', tiek.pavadinimas, ' | ', tiek.adresas) as tiekejas,
					CONCAT(uzsk.uz_nr, ' | ', uzsk.uzsakymo_data) as uzsakymas,                    
					mot.name as motininePav,
					korp.name as korpusasPav,
					ausin.name as ausinimasPav
				FROM
					`{Config.TblPrefix}kompiuteriai` komp
					LEFT JOIN `{Config.TblPrefix}tiekejai` tiek ON komp.fk_TIEKEJAS=tiek.tiekej_d
					LEFT JOIN `{Config.TblPrefix}uzsakymai` uzsk ON komp.fk_UZSAKYMAS=uzsk.uz_nr
					LEFT JOIN `{Config.TblPrefix}motininiu_tipai` mot ON komp.motinines_plokstes_tipas=mot.id_motininiu_tipai
					LEFT JOIN `{Config.TblPrefix}korpusu_tipai` korp ON komp.korpusas=korp.id_korpusu_tipai
					LEFT JOIN `{Config.TblPrefix}ausinimai` ausin ON komp.ausinimas=ausin.id_ausinimai
				WHERE
					komp.fk_UZSAKYMAS IS NULL
				ORDER BY
					komp.komp_id";
			// TODO: Galetu dar buti kietasis diskas

			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<KompiuterisL>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("komp_id");
					t.Pavadinimas = dre.From<string>("pavadinimas");
					t.Procesorius = dre.From<string>("procesorius");
					t.VaizdoPlokste = dre.From<string>("vaizdo_plokste");
					t.MaitinimoBlokas = dre.From<string>("maitinimo_blokas");
					t.FkTiekejas = dre.From<string>("tiekejas");
					t.FkUzsakymas = dre.From<string>("uzsakymas");
					t.MotininesPlokstesTipas = dre.From<string>("motininePav");
					t.Korpusas = dre.From<string>("korpusasPav");
					t.Ausinimas = dre.From<string>("ausinimasPav");
				});

			return result;
		}

		public static IList<KompiuterisCE.PriklausantysDiskaiM> ListKompiuterioAtmintys(string kompiuterisID)
		{
			var query =
				$@"SELECT 
					*
				FROM
					`{Config.TblPrefix}kietieji_diskai`
				WHERE
					fk_KOMPIUTERIS=?kompiuterisID
				ORDER BY
					Gamintojas ASC,
					Talpa ASC";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?kompiuterisID", kompiuterisID);
				});

			var result =
				Sql.MapAll<KompiuterisCE.PriklausantysDiskaiM>(drc, (dre, t) =>
				{
					t.Gamintojas = dre.From<string>("Gamintojas");
					t.Talpa = dre.From<int>("Talpa");
					t.Tipas = dre.From<string>("Tipas");
				});

			for (int i = 0; i < result.Count; i++)
			{
				result[i].inListID = i;
			}

			return result;
		}

		public static int InsertKompiuteris(KompiuterisCE kompCE)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}kompiuteriai`
				(
					komp_id,
					pavadinimas,
					procesorius,
					pagrindine_atmintis,
					vaizdo_plokste,
					maitinimo_blokas,
					motinines_plokstes_tipas,
					korpusas,
					ausinimas,
					fk_TIEKEJAS
				)
				VALUES
				(
					?komp_id,
					?pavadinimas,
					?procesorius,
					?pagrindine_atmintis,
					?vaizdo_plokste,
					?maitinimo_blokas,
					?motinines_plokstes_tipas,
					?korpusas,
					?ausinimas,
					?fk_TIEKEJAS
				)";

			var nr =
				Sql.Insert(query, args =>
				{
					// shortcut
					var auto = kompCE.Kompiuteris;

					args.Add("?komp_id", auto.Id);
					args.Add("?pavadinimas", auto.Pavadinimas);
					args.Add("?procesorius", auto.Procesorius);
					args.Add("?pagrindine_atmintis", auto.PagrindineAtmintis);
					args.Add("?vaizdo_plokste", auto.VaizdoPlokste);
					args.Add("?maitinimo_blokas", auto.MaitinimoBlokas);
					args.Add("?motinines_plokstes_tipas", auto.MotininesPlokstesTipas);
					args.Add("?korpusas", auto.Korpusas);
					args.Add("?ausinimas", auto.Ausinimas);
					args.Add("?fk_TIEKEJAS", auto.FkTiekejas);
				});

			return (int)nr;
		}

		public static KompiuterisCE FindKompiuterisCE(string id)
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}kompiuteriai` WHERE komp_id=?id";

			var drc =
				Sql.Query(query, args =>
				{
					args.Add("?id", id);
				});

			if (drc.Count > 0)
			{
				var result =
					Sql.MapOne<KompiuterisCE>(drc, (dre, t) => {
						// shortcut
						var auto = t.Kompiuteris;

						auto.Id = dre.From<int>("komp_id");
						auto.Pavadinimas = dre.From<string>("pavadinimas");
						auto.Procesorius = dre.From<string>("procesorius");
						auto.PagrindineAtmintis = dre.From<string>("pagrindine_atmintis");
						auto.VaizdoPlokste = dre.From<string>("vaizdo_plokste");
						auto.MaitinimoBlokas = dre.From<string>("maitinimo_blokas");
						auto.MotininesPlokstesTipas = dre.From<int>("motinines_plokstes_tipas");
						auto.Korpusas = dre.From<int>("korpusas");
						auto.Ausinimas = dre.From<int>("ausinimas");
						auto.FkTiekejas = dre.From<int>("fk_TIEKEJAS");
						auto.FkUzsakymas = dre.From<string>("fk_UZSAKYMAS");
					});

				return result;
			}

			return null;
		}

		public static void Update(KompiuterisCE kompCE)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}kompiuteriai`
				SET
					pavadinimas = ?pavadinimas,
					procesorius = ?procesorius,
					pagrindine_atmintis = ?pagrindine_atmintis,
					vaizdo_plokste = ?vaizdo_plokste,
					maitinimo_blokas = ?maitinimo_blokas,
					motinines_plokstes_tipas = ?motinines_plokstes_tipas,
					korpusas = ?korpusas,
					ausinimas = ?ausinimas,
					fk_TIEKEJAS = ?fk_TIEKEJAS,
					fk_UZSAKYMAS = ?fk_UZSAKYMAS
				WHERE
					komp_id=?id";

			Sql.Update(query, args =>
			{
				// shortcut
				var auto = kompCE.Kompiuteris;

				args.Add("?id", auto.Id);
				args.Add("?pavadinimas", auto.Pavadinimas);
				args.Add("?procesorius", auto.Procesorius);
				args.Add("?pagrindine_atmintis", auto.PagrindineAtmintis);
				args.Add("?vaizdo_plokste", auto.VaizdoPlokste);
				args.Add("?maitinimo_blokas", auto.MaitinimoBlokas);
				args.Add("?motinines_plokstes_tipas", auto.MotininesPlokstesTipas);
				args.Add("?korpusas", auto.Korpusas);
				args.Add("?ausinimas", auto.Ausinimas);
				args.Add("?fk_TIEKEJAS", auto.FkTiekejas);
				args.Add("?fk_UZSAKYMAS", auto.FkUzsakymas);
			});
		}

		public static void DeleteKompiuteris(string id)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}kompiuteriai` WHERE komp_id=?id";
			Sql.Delete(query, args =>
			{
				args.Add("?id", id);
			});
		}

		public static List<AusinimuTipai> ListAusinimuTipai()
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}ausinimai` ORDER BY id_ausinimai ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<AusinimuTipai>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("id_ausinimai");
					t.Pavadinimas = dre.From<string>("name");
				});

			return result;
		}

		public static List<MotininiuTipai> ListMotininiuTipai()
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}motininiu_tipai` ORDER BY id_motininiu_tipai ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<MotininiuTipai>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("id_motininiu_tipai");
					t.Pavadinimas = dre.From<string>("name");
				});

			return result;
		}

		public static List<KorpusuTipai> ListKorpusuTipai()
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}korpusu_tipai` ORDER BY id_korpusu_tipai ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<KorpusuTipai>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("id_korpusu_tipai");
					t.Pavadinimas = dre.From<string>("name");
				});

			return result;
		}

		public static List<AtmintiesTipai> ListAtmintiesTipai()
		{
			var query = $@"SELECT * FROM `{Config.TblPrefix}atminties_tipai` ORDER BY id_atminties_tipai ASC";
			var drc = Sql.Query(query);

			var result =
				Sql.MapAll<AtmintiesTipai>(drc, (dre, t) =>
				{
					t.Id = dre.From<int>("id_atminties_tipai");
					t.Pavadinimas = dre.From<string>("name");
				});

			return result;
		}

		internal static void UpdateKompiuteris(KompiuterisCE kompCE)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}kompiuteriai`
				SET
					pavadinimas=?pavadinimas,
					procesorius=?procesorius,
					pagrindine_atmintis=?pagrindine_atmintis,
					vaizdo_plokste=?vaizdo_plokste,
					maitinimo_blokas=?maitinimo_blokas,
					motinines_plokstes_tipas=?motinines_plokstes_tipas,
					korpusas=?korpusas,
					ausinimas=?ausinimas,
					fk_TIEKEJAS=?fk_TIEKEJAS,
					fk_UZSAKYMAS=?fk_UZSAKYMAS
				WHERE
					komp_id=?komp_id";

			Sql.Update(query, args =>
			{
				// shorcut
				var komp = kompCE.Kompiuteris;

				args.Add("?pavadinimas", komp.Pavadinimas);
				args.Add("?procesorius", komp.Procesorius);
				args.Add("?pagrindine_atmintis", komp.PagrindineAtmintis);
				args.Add("?vaizdo_plokste", komp.VaizdoPlokste);
				args.Add("?maitinimo_blokas", komp.MaitinimoBlokas);
				args.Add("?motinines_plokstes_tipas", komp.MotininesPlokstesTipas);
				args.Add("?korpusas", komp.Korpusas);
				args.Add("?ausinimas", komp.Ausinimas);
				args.Add("?fk_TIEKEJAS", komp.FkTiekejas);
				args.Add("?fk_UZSAKYMAS", komp.FkUzsakymas);

				args.Add("?komp_id", komp.Id);
			});
		}

		internal static void DeleteAtmintisForKompiuteris(KompiuterisCE kompCE)
		{
			int kompID = kompCE.Kompiuteris.Id;

			var query = $@"DELETE FROM
							`{Config.TblPrefix}kietieji_diskai`
						WHERE
							fk_KOMPIUTERIS=?kompID";

			Sql.Delete(query, args =>
			{
				args.Add("?kompID", kompID);
			});
		}

		internal static void InsertAtmintis(int kompID, KompiuterisCE.PriklausantysDiskaiM diskas)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}kietieji_diskai`
				(
					Gamintojas,
					Talpa,
					Tipas,
					id_KIETASIS_DISKAS,
					fk_KOMPIUTERIS
				)
				VALUES
				(
					?Gamintojas,
					?Talpa,
					?Tipas,
					?id_KIETASIS_DISKAS,
					?fk_KOMPIUTERIS
				)";

			Sql.Insert(query, args =>
			{
				args.Add("?Gamintojas", diskas.Gamintojas);
				args.Add("?Talpa", diskas.Talpa);
				args.Add("?Tipas", diskas.Tipas);
				args.Add("?id_KIETASIS_DISKAS", 0);
				args.Add("?fk_KOMPIUTERIS", kompID);
			});
		}
	}
}
