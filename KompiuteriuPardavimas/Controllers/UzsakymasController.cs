using KompiuteriuPardavimas.Models;
using KompiuteriuPardavimas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

namespace KompiuteriuPardavimas.Controllers
{
	public class UzsakymasController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			//return View();
			return View(UzsakymasRepository.ListUzsakymas());
		}

		[HttpGet]
		public ActionResult Create()
		{
			var uzs = new UzsakymasCE();

			uzs.Uzsakymas.UzsakymoData = DateTime.Now;

			PopulateLists(uzs, true);

			return View(uzs);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form
		/// </summary>
		/// <param name="save">If not null, indicates that 'Save' button was clicked</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove</param>
		/// <param name="uzsCE">Entity view moel filled with latest data</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add_papild, int? add_komp, int? remove_papild, int? remove_komp, UzsakymasCE uzsCE)
		{
			// addition of new 'Kompiuteris' record was requested?
			if ( add_komp != null )
			{
				// add entry for the new record
				var komp =
					new UzsakymasCE.UzsakytasKompiuterisM
					{
						inListID =
							uzsCE.UzsakytiKompiuteriai.Count > 0 ?
							uzsCE.UzsakytiKompiuteriai.Max(it => it.inListID) + 1 :
							0,
						Pavadinimas = null
					};
				uzsCE.UzsakytiKompiuteriai.Add(komp);

				// Make sure @Html helper is not reusing old model state containing old list
				ModelState.Clear();

				// Go back to the form
				PopulateLists(uzsCE, true);
				return View(uzsCE);
			}
			// removal of existing 'Kompiuteris' record was requested?
			if ( remove_komp != null )
			{
				// filter out 'Kompiuteris' record having in-list-id the same as the given one
				uzsCE.UzsakytiKompiuteriai =
					uzsCE
						.UzsakytiKompiuteriai
						.Where(it => it.inListID != remove_komp.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists(uzsCE, true);
				return View(uzsCE);
			}

            // addition of new 'Papildomas Mokestis' record was requested?
            if (add_papild != null)
            {
				// add entry for the new record
				var papild =
					new UzsakymasCE.UzsakytasPapildomasMokestisM
					{
						inListID =
							uzsCE.UzsakytiPapildomiMokesciai.Count > 0 ?
							uzsCE.UzsakytiPapildomiMokesciai.Max(it => it.inListID) + 1 :
							0,
						Pavadinimas = null,
						IDMokestis = 0,
						IDUzsakymas = 0,
						Kaina = 0,
						Kiekis = 0
					};
				uzsCE.UzsakytiPapildomiMokesciai.Add(papild);

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists(uzsCE, true);
				return View(uzsCE);
            }

            // removal of existing 'Papildomas Mokestis' record was requested?
            if (remove_papild != null)
            {
				// filter out 'Papildomas Mokestis' record having in-list-id the same as the given one
				uzsCE.UzsakytiPapildomiMokesciai =
					uzsCE
						.UzsakytiPapildomiMokesciai
						.Where(it => it.inListID != remove_papild.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists (uzsCE, true);
				return View(uzsCE);
            }

            // save of the form data was requested?
            if (save != null)
			{
				// check for attempts to create duplicate 'Papildomas Mokestis' records
				for (int i = 0; i < uzsCE.UzsakytiPapildomiMokesciai.Count - 1; i++)
				{
					var refKomp = uzsCE.UzsakytiPapildomiMokesciai[i];

					for (int j = i + 1; j < uzsCE.UzsakytiPapildomiMokesciai.Count; j++)
					{
						var testKomp = uzsCE.UzsakytiPapildomiMokesciai[j];

						if (testKomp.Pavadinimas == refKomp.Pavadinimas && refKomp != null)
							ModelState.AddModelError($"UzsakytiPapildomiMokesciai[{j}]", "Duplicate of another added computer.");
					}
				}

				// form field validation passed?
				if (ModelState.IsValid)
				{
					// create new 'Uzsakymas'
					uzsCE.Uzsakymas.Id = UzsakymasRepository.InsertUzsakymas(uzsCE);

					// create new 'Kompiuteris' records
					foreach (var upKomp in uzsCE.UzsakytiKompiuteriai)
						UzsakymasRepository.UpdateKompiuteris(uzsCE.Uzsakymas.Id, upKomp.Pavadinimas);

					// add IDS
					for (int i = 0; i < uzsCE.UzsakytiPapildomiMokesciai.Count; i++)
					{
						uzsCE.UzsakytiPapildomiMokesciai[i].IDUzsakymas = uzsCE.Uzsakymas.Id;
						uzsCE.UzsakytiPapildomiMokesciai[i].IDMokestis = Int32.Parse(uzsCE.UzsakytiPapildomiMokesciai[i].Pavadinimas);
					}

                    // create new 'Papildomi Mokesciai' records
                    foreach (var upPapild in uzsCE.UzsakytiPapildomiMokesciai)
						UzsakymasRepository.InsertPapildomasMokestis(uzsCE.Uzsakymas.Id, upPapild);

                    // save success, go back to the entity list
                    return RedirectToAction("Index");
				}
				// form field validation failed, go back to the form
				else
				{
					PopulateLists(uzsCE, true);
					return View(uzsCE);
				}
			}

			// should not reach here
			throw new Exception("Unexpected error in UzsakymasController.cs");
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser
		/// </summary>
		/// <param name="id">ID of the entity to edit</param>
		/// <returns>Editing form view</returns>
		[HttpGet]
		public ActionResult Edit(int id)
		{
			var uzsCE = UzsakymasRepository.FindUzsakymas(id);

			uzsCE.UzsakytiKompiuteriai = UzsakymasRepository.ListUzsakytiKompiuteriai(id);
			uzsCE.UzsakytiPapildomiMokesciai = UzsakymasRepository.ListPapildomiMokesciai(id);

			PopulateLists(uzsCE, true);

			return View(uzsCE);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked</param>
		/// <param name="add_papild">If not null, indicates that 'Papildomi mokesciai' 'Add' button was clicked</param>
		/// <param name="add_komp">If not null, indicates that 'Kompiuteriai' 'Add' button was clicked</param>
		/// <param name="remove_papild">If not null, indicates that 'Papildomi mokesciai' 'Remove' button was clicked></param>
		/// <param name="remove_komp">If not null, indicates that 'Kompiuteriai' 'Add' button was clicked</param>
		/// <param name="uzsCE">Entity view model filled with latest data</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull</returns>
		[HttpPost]
		public ActionResult Edit(int id, int? save, int? add_papild, int? add_komp, int? remove_papild, int? remove_komp, UzsakymasCE uzsCE)
		{
			// Addition of new 'Kompiuteris' record was requested?
			if (add_komp != null)
			{
				// add entry for the new record
				var komp =
					new UzsakymasCE.UzsakytasKompiuterisM
					{
						inListID =
							uzsCE.UzsakytiKompiuteriai.Count > 0 ?
							uzsCE.UzsakytiKompiuteriai.Max(it => it.inListID) + 1 :
							0,
						Pavadinimas = null
					};
				uzsCE.UzsakytiKompiuteriai.Add(komp);

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists(uzsCE, true);
				return View(uzsCE);
			}

            // Addition of new 'Papildomas Mokestis' record was requested?
            if (add_papild != null)
            {
				// add entry for the new record
				var papild =
					new UzsakymasCE.UzsakytasPapildomasMokestisM
					{
						inListID =
							uzsCE.UzsakytiPapildomiMokesciai.Count > 0 ?
							uzsCE.UzsakytiPapildomiMokesciai.Max(it => it.inListID) + 1 :
							0,
						Pavadinimas = null,
						Aprasymas = null,
						Kaina = 0,
						Kiekis = 0
					};
				uzsCE.UzsakytiPapildomiMokesciai.Add(papild);

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists (uzsCE, true);
				return View(uzsCE);
            }

            // removal of existing 'Kompiuteris' record was requested?
            if (remove_komp != null)
			{
				uzsCE.UzsakytiKompiuteriai =
					uzsCE
						.UzsakytiKompiuteriai
						.Where(it => it.inListID != remove_komp.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists(uzsCE, true);
				return View(uzsCE);
			}

            // removal of existing 'Papildomas Mokestis' record requested?
            if (remove_papild != null)
            {
                uzsCE.UzsakytiPapildomiMokesciai =
					uzsCE
						.UzsakytiPapildomiMokesciai
						.Where(it => it.inListID != remove_papild.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateLists(uzsCE, false);
				return View(uzsCE);
            }

            // save of the from data was requested?
            if (save != null)
			{
				// check for attempts to create duplicate 'Kompiuteris' records
				for (var i = 0; i < uzsCE.UzsakytiKompiuteriai.Count-1; i++)
				{
					var refUp = uzsCE.UzsakytiKompiuteriai[i];

					for (var j = i+1; j < uzsCE.UzsakytiKompiuteriai.Count; j++)
					{
						var testUp = uzsCE.UzsakytiKompiuteriai[j];

						if (testUp.Pavadinimas == refUp.Pavadinimas)
						{
							ModelState.AddModelError($"UzsakytiKompiuteriai[{j}].Pavadinimas", "Duplicate of another added computer");
						}
					}
				}

				// check for attempts to create duplicate 'Papildomas Mokestis' records
				for (var i = 0; i < uzsCE.UzsakytiPapildomiMokesciai.Count - 1; i++)
				{
					var refPapild = uzsCE.UzsakytiPapildomiMokesciai[i];

					for (var j = i + 1; j < uzsCE.UzsakytiPapildomiMokesciai.Count; j++)
					{
						var testPapild = uzsCE.UzsakytiPapildomiMokesciai[i];
						if (testPapild.Pavadinimas == refPapild.Pavadinimas &&
							testPapild.Aprasymas == testPapild.Aprasymas)
						{
							ModelState.AddModelError($"UzsakytiPapildomiMokesciai[{j}]", "Duplicate of another added fee");
                        }
					}
				}

				// form field validation passed?
				if (ModelState.IsValid)
				{
					// update 'Uzsakymas'
					UzsakymasRepository.UpdateUzsakymas(uzsCE);

					// delete all old 'UzsakytiKompiuteriai' and 'Papildomas Mokestis' records
					UzsakymasRepository.DeleteUzsakytiKompiuteriaiForUzsakymas(uzsCE.Uzsakymas.Id);
					UzsakymasRepository.DeletePapildomiMokesciaiForUzsakymas(uzsCE.Uzsakymas.Id);

					// create new 'UzsakytiKompiuteriai' and 'Papildomas Mokestis' records
					foreach (var upVm in uzsCE.UzsakytiKompiuteriai)
					{
						UzsakymasRepository.UpdateKompiuteris(uzsCE.Uzsakymas.Id, upVm.Pavadinimas);
					}

                    // add IDS
                    for (int i = 0; i < uzsCE.UzsakytiPapildomiMokesciai.Count; i++)
                    {
                        uzsCE.UzsakytiPapildomiMokesciai[i].IDUzsakymas = uzsCE.Uzsakymas.Id;
                        uzsCE.UzsakytiPapildomiMokesciai[i].IDMokestis = Int32.Parse(uzsCE.UzsakytiPapildomiMokesciai[i].Pavadinimas);
                    }

                    foreach (var upMok in uzsCE.UzsakytiPapildomiMokesciai)
                    {
						UzsakymasRepository.InsertPapildomasMokestis(uzsCE.Uzsakymas.Id, upMok);
                    }

                    // save success, go back to the entity list
                    return RedirectToAction("Index");
				}
				// form field validation failed, go back to the form
				else
				{
					PopulateLists(uzsCE, true);
					return View(uzsCE);
				}
			}

			// should not reach here
			throw new Exception("Unexpected error in UzsakymasController.cs");
		}


		private void PopulateLists(UzsakymasCE uzs, bool freeKomp)
		{
			// load entities for the select lists
			List<KompiuterisL> kompiuteriai = new List<KompiuterisL>();
			if (freeKomp)
			{
				kompiuteriai = KompiuterisRepository.ListFreeKompiuteris();
			} else
			{
				kompiuteriai = KompiuterisRepository.ListKompiuteris();
			}
			var klientai = KlientasRepository.List();
			var darbuotojai = DarbuotojasRepository.List();
			var mokesciai = UzsakymasRepository.ListPapildomiMokesciai();

			// build select lists
			uzs.Lists.Kompiuteriai =
				kompiuteriai
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = Convert.ToString(it.Id),
								Text = $"{it.Pavadinimas}"
							};
					})
					.ToList();

			uzs.Lists.Klientai =
				klientai
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = Convert.ToString(it.Id),
								Text = $"{it.Vardas} {it.Pavarde}"
							};
					})
					.ToList();

			uzs.Lists.Darbuotojai =
				darbuotojai
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = Convert.ToString(it.Kodas),
								Text = $"{it.Vardas} {it.Pavarde}"
							};
					})
					.ToList();

			// build select list for 'Mokesciai'
			uzs.Lists.Mokesciai =
				mokesciai
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = Convert.ToString(it.IDMokestis),
								Text = $"{it.Pavadinimas} | {it.Aprasymas}"
							};
					})
					.ToList();
				
		}
	}
}
