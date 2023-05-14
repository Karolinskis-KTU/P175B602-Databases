using KompiuteriuPardavimas.Models;
using KompiuteriuPardavimas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KompiuteriuPardavimas.Controllers
{
	public class KompiuterisController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View(KompiuterisRepository.ListKompiuteris());
		}

		/// <summary>
		/// This is invoked when creaton form is first opened in browser
		/// </summary>
		/// <returns>Creation form view</returns>
		[HttpGet]
		public ActionResult Create()
		{
			var user = new KompiuterisCE();
			PopulateSelections(user);
			return View(user);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="kompiuteris">Entity model filled with latest data</param>
		/// <returns>Returns creation from view or redirects back to Index if save was successfull</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add, int? remove, KompiuterisCE kompCE)
		{
            // Addition of 'Atmintis' record was requested?
            if (add != null)
            {
				// add entry for the new record
				var atmintis =
					new KompiuterisCE.PriklausantysDiskaiM
					{
						inListID =
							kompCE.KompiuterioKietiejiDiskai.Count > 0 ?
							kompCE.KompiuterioKietiejiDiskai.Max(it => it.inListID) + 1 :
							0,
						Gamintojas = null,
						Talpa = 0,
						Tipas = null,
					};
				kompCE.KompiuterioKietiejiDiskai.Add(atmintis);

				// Make sure @Html helper is not reusing old model state containing old list
				ModelState.Clear();

				// Go back to the form
				PopulateSelections(kompCE);
				return View(kompCE);
            }

            // removal of existing 'Atmintis' record was requested?
            if (remove != null)
            {
                // filter out 'Atmintis' record having in-list-id the same as the given one
				kompCE.KompiuterioKietiejiDiskai =
					kompCE
						.KompiuterioKietiejiDiskai
						.Where(it => it.inListID != remove.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateSelections(kompCE);
				return View(kompCE);
            }

            // save of the form data was requested?
            if (save != null)
            {
				//// check for attempts to create duplicate 'Atmintis' records
				//for (int i = 0; i < kompCE.KompiuterioKietiejiDiskai.Count-1; i++)
				//{
				//	var refKiet = kompCE.KompiuterioKietiejiDiskai[i];

				//	for (int j = i + 1; j < kompCE.KompiuterioKietiejiDiskai.Count; j++)
				//	{
				//		var testKiet = kompCE.KompiuterioKietiejiDiskai[j];

				//		if (refKiet.Gamintojas == testKiet.Gamintojas &&
				//			refKiet.Talpa == testKiet.Talpa &&
				//			refKiet.Tipas == testKiet.Tipas)
				//		{
				//			ModelState.AddModelError($"KompiuterioKietiejiDiskai[{j}]",
				//				"Duplicate of another added drive");
				//		}
				//	}
				//}

				// form field validation passed?
				if (ModelState.IsValid)
				{
					// create new 'Kompiuteris'
					kompCE.Kompiuteris.Id = KompiuterisRepository.InsertKompiuteris(kompCE);

                    // create new 'Atmintis' records
                    foreach (var upAtm in kompCE.KompiuterioKietiejiDiskai)
                    {
						KompiuterisRepository.InsertAtmintis(kompCE.Kompiuteris.Id, upAtm);
                    }

					// save success, go back to the entity list
					return RedirectToAction("Index");
                }
				// form field validation failed, go back to the form
				else
				{
					PopulateSelections(kompCE);
					return View(kompCE);
				}
            }

			// should not reach here
			throw new Exception("Unexpected error in KompiuterisController.cs, Create()");
        }

		[HttpGet]
		public ActionResult Edit(string id)
		{
			var kompCE = KompiuterisRepository.FindKompiuterisCE(id);

			kompCE.KompiuterioKietiejiDiskai = KompiuterisRepository.ListKompiuterioAtmintys(id);
			PopulateSelections(kompCE);

			return View(kompCE);
		}

		/// <summary>
		/// This is invoked when buttons are pressend in the editing form
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked</param>
		/// <param name="kompCE">Entity view model filled with latest data</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull</returns>
		[HttpPost]
		public ActionResult Edit(string id, int? save, int? add, int? remove, KompiuterisCE kompCE)
		{
			// Addition of new 'Atmintis' record was requested?
			if (add != null)
			{
				// add entry for the new record
				var atmintis =
					new KompiuterisCE.PriklausantysDiskaiM
					{
						inListID =
							kompCE.KompiuterioKietiejiDiskai.Count > 0 ?
							kompCE.KompiuterioKietiejiDiskai.Max(it => it.inListID) + 1 :
							0,
						Gamintojas = null,
						Talpa = 0,
						Tipas = null,
					};
				kompCE.KompiuterioKietiejiDiskai.Add(atmintis);

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateSelections(kompCE);
				return View(kompCE);
			}

			// Removal of existing 'Atmintis record was requested?
			if (remove != null)
			{
				kompCE.KompiuterioKietiejiDiskai =
					kompCE
						.KompiuterioKietiejiDiskai
						.Where(it => it.inListID != remove.Value)
						.ToList();

				// make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				// go back to the form
				PopulateSelections(kompCE);
				return View(kompCE);
			}

			// save of the form data was requested?
			if (save != null)
			{
				// check for attempts to create duplicate 'Atmintis' records
				for (var i = 0; i < kompCE.KompiuterioKietiejiDiskai.Count - 1; i++) 
				{
					var refUp = kompCE.KompiuterioKietiejiDiskai[i];

					for (int j = i + 1; j < kompCE.KompiuterioKietiejiDiskai.Count; j++)
					{
						var testUp = kompCE.KompiuterioKietiejiDiskai[j];

						if (testUp.Gamintojas == refUp.Gamintojas &&
							testUp.Talpa == refUp.Talpa &&
							testUp.Tipas == testUp.Tipas)
						{
							ModelState.AddModelError($"KompiuterioKietiejiDiskai[{j}]",
								"Duplicate of another added drive");
						}
					}
				}

				// form field validation passed?
				if (ModelState.IsValid)
				{
					// update 'Kompiuteris'
					KompiuterisRepository.UpdateKompiuteris(kompCE);

					// delete all old 'Atmintis' records
					KompiuterisRepository.DeleteAtmintisForKompiuteris(kompCE);

					// create new 'Atmintis' records
					foreach (var upVm in kompCE.KompiuterioKietiejiDiskai)
					{
						KompiuterisRepository.InsertAtmintis(kompCE.Kompiuteris.Id, upVm);
					}

					// save success, go back to the entity list
					return RedirectToAction("Index");
				}
				// form field validation failed, go back to the form
				else
				{
					PopulateSelections(kompCE);
					return View(kompCE);
				}
			}

			throw new Exception("Unexpected error in KompiuterisController.cs, Edit()");
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string id)
		{
			// try deleting, this will fial if foreign key constraint fails
			try
			{
				KompiuterisRepository.DeleteKompiuteris(id);

				// deletion success, redirect to list form
				return RedirectToAction("Index");
			}
			// entity in use, deletion not permitted
			catch (MySql.Data.MySqlClient.MySqlException)
			{
				// enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var kompiuteris = KompiuterisRepository.FindKompiuterisCE(id);
				return View("Delete", kompiuteris);
			}
		}

		/// <summary>
		/// Populates select lists use to render drop down controls
		/// </summary>
		/// <param name="kompCE">'Kompiuteris' view model to append to</param>
		public void PopulateSelections(KompiuterisCE kompCE)
		{
			// load entities for the select lists
			var ausinimai = KompiuterisRepository.ListAusinimuTipai();
			var motinines = KompiuterisRepository.ListMotininiuTipai();
			var korpusai = KompiuterisRepository.ListKorpusuTipai();
			var atmintys = KompiuterisRepository.ListAtmintiesTipai();
			var tiekejai = TiekejasRepository.List();

			// build select lists
			kompCE.Lists.Ausinimai =
				ausinimai.Select(it =>
				{
					return
						new SelectListItem()
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			kompCE.Lists.Motinines =
				motinines.Select(it =>
				{
					return
						new SelectListItem()
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			kompCE.Lists.Korpusai =
				korpusai.Select(it =>
				{
					return
						new SelectListItem()
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			kompCE.Lists.Atmintys =
				atmintys.Select(it =>
				{
					return
						new SelectListItem()
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			kompCE.Lists.Tiekejai =
				tiekejai.Select(it =>
				{
					return
						new SelectListItem()
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();
		}
	}
}
