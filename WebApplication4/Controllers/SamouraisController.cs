using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using WebApplication4.Data;

namespace WebApplication4.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM  samouraiVM= new SamouraiVM();
            samouraiVM.Samourai = samourai;
            if(samourai.Arme != null)
            {
                samouraiVM.potentiel = samourai.Force * samourai.Arme.Degats * (samourai.ArtMartiaux.Count + 1);
            }
            else
            {
                samouraiVM.potentiel = samourai.Force * (samourai.ArtMartiaux.Count + 1);
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            var samouraiVm = new SamouraiVM();
            samouraiVm.ListArmes = db.Armes.Where(a=>a.Samourai == null).ToList();
            samouraiVm.ListArtM = db.ArtMartials.ToList();
            return View(samouraiVm);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            var samourai = samouraiVM.Samourai;
            if(samouraiVM.IdSelected != null)
            {
                samourai.Arme = db.Armes.Find(samouraiVM.IdSelected);
            }
            if (samouraiVM.IdSelectedArtM.Count > 0)
            {
                samourai.Arme = db.Armes.Find(samouraiVM.IdSelected);
                samouraiVM.IdSelectedArtM.ForEach(i => samourai.ArtMartiaux.Add(db.ArtMartials.Find(i)));
            }

            if (ModelState.IsValid)
            {
                db.Samourais.Add(samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(samourai);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            var samouraiVM = new SamouraiVM();
            samouraiVM.ListArmes = db.Armes.Where(a => a.Samourai == null).ToList();
            samouraiVM.ListArtM = db.ArtMartials.ToList();
            samouraiVM.Samourai = samourai;
            if ( samourai.Arme != null)
            {
                samouraiVM.IdSelected = samourai.Arme.Id;
            }

            if (samourai.ArtMartiaux.Count > 0 )
            {
                samourai.ArtMartiaux.ForEach(a => samouraiVM.IdSelectedArtM.Add(a.Id));
            }


            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                var samourail = db.Samourais.Find(samouraiVM.Samourai.Id);
                samourail.Force = samouraiVM.Samourai.Force;
                samourail.Nom = samouraiVM.Samourai.Nom;
                samourail.Arme = db.Armes.Find(samouraiVM.IdSelected);
                samourail.ArtMartiaux.Clear();
                samouraiVM.IdSelectedArtM.ForEach(i => samourail.ArtMartiaux.Add(db.ArtMartials.Find(i)));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM samouraiVM = new SamouraiVM();
            samouraiVM.Samourai = samourai;
            if (samourai.Arme != null)
            {
                samouraiVM.potentiel = samourai.Force * samourai.Arme.Degats * (samourai.ArtMartiaux.Count + 1);
            }
            else
            {
                samouraiVM.potentiel = samourai.Force * (samourai.ArtMartiaux.Count + 1);
            }
            return View(samouraiVM);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            samourai.ArtMartiaux.Clear();
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
