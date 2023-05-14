using KompiuteriuPardavimas.Models;
using KompiuteriuPardavimas.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KompiuteriuPardavimas.Controllers
{
    public class TiekejasController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(TiekejasRepository.List());
        }

        /// <summary>
        /// This is invoked when creaton form is first opened in browser
        /// </summary>
        /// <returns>Creation form view</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var user = new Tiekejas();
            return View(user);
        }

        /// <summary>
        /// This is invoked when buttons are pressed in the creation form.
        /// </summary>
        /// <param name="tiekejas">Entity model filled with latest data</param>
        /// <returns>Returns creation from view or redirects back to Index if save was successfull</returns>
        [HttpPost]
        public ActionResult Create(Tiekejas tiekejas)
        {
            // form field validation passed?
            if (ModelState.IsValid)
            {
                TiekejasRepository.Insert(tiekejas);

                // save success, go back to the entity list
                return RedirectToAction("Index");
            }

            // form field validation failed, go back to the form
            return View(tiekejas);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var tiek = TiekejasRepository.Find(id);
            return View(tiek);
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
                TiekejasRepository.Delete(id);

                // deletion success, redirect to list form
                return RedirectToAction("Index");
            }
            // entity in use, deletion not permitted
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                // enable explanatory message and show delete form
                ViewData["deletionNotPermitted"] = true;

                var tiekejas = TiekejasRepository.Find(id);
                return View("Delete", tiekejas);
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(TiekejasRepository.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(string id, Tiekejas tiekejas)
        {
            // form field validation passed?
            if (ModelState.IsValid)
            {
                TiekejasRepository.Update(tiekejas);

                // save success, go back to the entity list
                return RedirectToAction("Index");
            }

            // form field validation failed, go back to the form
            return View(tiekejas);
        }
    }
}
