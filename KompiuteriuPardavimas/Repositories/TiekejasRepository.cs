using KompiuteriuPardavimas.Models;

namespace KompiuteriuPardavimas.Repositories
{
    public class TiekejasRepository
    {
        public static List<Tiekejas> List()
        {
            var query = $@"SELECT * FROM `{Config.TblPrefix}tiekejai` ORDER BY tiekej_d ASC";
            var sqlQueryData = Sql.Query(query);

            var result =
                Sql.MapAll<Tiekejas>(sqlQueryData, (dre, t) =>
                {
                    t.Id = dre.From<int>("tiekej_d");
                    t.Pavadinimas = dre.From<string>("pavadinimas");
                    t.Telefonas = dre.From<string>("telefonas");
                    t.ElPastas = dre.From<string>("el_pastas");
                    t.Adresas = dre.From<string>("adresas");
                });

            return result;
        }

        public static void Insert(Tiekejas tiekejas)
        {
            var query =
                $@"INSERT INTO `{Config.TblPrefix}tiekejai`
                (
                    tiekej_d,
                    pavadinimas,
                    telefonas,
                    el_pastas,
                    adresas
                )
                VALUES
                (
                    ?tiekej_d,
                    ?pavadinimas,
                    ?telefonas,
                    ?el_pastas,
                    ?adresas
                )";

            Sql.Query(query, args =>
            {
                args.Add("?tiekej_d", tiekejas.Id);
                args.Add("?pavadinimas", tiekejas.Pavadinimas);
                args.Add("?telefonas", tiekejas.Telefonas);
                args.Add("?el_pastas", tiekejas.ElPastas);
                args.Add("?adresas", tiekejas.Adresas);
            });
        }

        /// <summary>
        /// Finds the distributor in the DB by their id
        /// </summary>
        /// <param name="id">ID to find by</param>
        /// <returns>Returns the distributor</returns>
        public static Tiekejas Find(string id)
        {
            var query = $@"SELECT * FROM `{Config.TblPrefix}tiekejai` WHERE tiekej_d=?id";

            var drc =
                Sql.Query(query, args =>
                {
                    args.Add("?id", id);
                });

            if (drc.Count > 0)
            {
                var result =
                    Sql.MapOne<Tiekejas>(drc, (dre, t) =>
                    {
                        t.Id = dre.From<int>("tiekej_d");
                        t.Pavadinimas = dre.From<string>("pavadinimas");
                        t.Telefonas = dre.From<string>("telefonas");
                        t.ElPastas = dre.From<string>("el_pastas");
                        t.Adresas = dre.From<string>("adresas");
                    });
                return result;
            }

            return null;
        }

        public static void Update(Tiekejas tiekejas)
        {
            var query =
                $@"UPDATE `{Config.TblPrefix}tiekejai`
                SET
                    pavadinimas = ?pavadinimas,
                    telefonas = ?telefonas,
                    el_pastas = ?el_pastas,
                    adresas = ?adresas
                WHERE
                    tiekej_d=?id";

            Sql.Update(query, args =>
            {
                args.Add("?id", tiekejas.Id);
                args.Add("?pavadinimas", tiekejas.Pavadinimas);
                args.Add("?telefonas", tiekejas.Telefonas);
                args.Add("?el_pastas", tiekejas.ElPastas);
                args.Add("?adresas", tiekejas.Adresas);
            });
        }

        public static void Delete(string id)
        {
            var query = $@"DELETE FROM `{Config.TblPrefix}tiekejai` WHERE tiekej_d=?id";
            Sql.Delete(query, args =>
            {
                args.Add("?id", id);
            });
        }
    }
}
