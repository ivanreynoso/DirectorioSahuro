using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectorioSahuro.Models;
using System.IO;
using DevExpress.Web.Mvc;

namespace DirectorioSahuro.Controllers
{
    public class DirectorioController : Controller
    {
        private DirectorioDBContext db = new DirectorioDBContext();

        //
        // GET: /Directorio/

        public ViewResult Index()
        {
            return View(db.Directorio.OrderBy(a => a.NOMBRE).ToList());
        }

        //
        // GET: /Directorio/Details/5

        public ViewResult Details(int id)
        {
            Directorio directorio = db.Directorio.Find(id);
            return View(directorio);
        }

        //
        // GET: /Directorio/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Directorio/Create

        [HttpPost]
        public ActionResult Create(Directorio directorio, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Directorio.Add(directorio);
                db.SaveChanges();
                //Saving image
                if (file != null)
                {
                    string pic = directorio.ID.ToString() + "." + System.IO.Path.GetFileName(file.ContentType);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/ProfileImages"), pic);
                    file.SaveAs(path);
                    directorio.FOTOGRAFIA = "Content/ProfileImages/" + pic + "";
                    db.SaveChanges();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                else
                {
                    directorio.FOTOGRAFIA = "Content/ProfileImages/no_image.png";
                    db.SaveChanges();
                }
                //
                return RedirectToAction("Index");
            }

            return View(directorio);
        }

        //
        // GET: /Directorio/Edit/5

        public ActionResult Edit(int id)
        {
            Directorio directorio = db.Directorio.Find(id);
            return View(directorio);
        }

        //
        // POST: /Directorio/Edit/5

        [HttpPost]
        public ActionResult Edit(Directorio directorio, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(directorio).State = EntityState.Modified;
                db.SaveChanges();
                //Delete actual image and save the new one
                if (!directorio.FOTOGRAFIA.Contains("no_image.png"))
                {
                    System.IO.File.Delete(Server.MapPath("~/" + directorio.FOTOGRAFIA));
                }
                if (file != null)
                {
                    string pic = directorio.ID.ToString() + "." + System.IO.Path.GetFileName(file.ContentType);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/ProfileImages"), pic);
                    file.SaveAs(path);
                    directorio.FOTOGRAFIA = "Content/ProfileImages/" + pic + "";
                    db.SaveChanges();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                else
                {
                    directorio.FOTOGRAFIA = "Content/ProfileImages/no_image.png";
                    db.SaveChanges();
                }
                //
                return RedirectToAction("Index");
            }
            return View(directorio);
        }

        //
        // GET: /Directorio/Delete/5

        public ActionResult Delete(int id)
        {
            Directorio directorio = db.Directorio.Find(id);
            return View(directorio);
        }

        //
        // POST: /Directorio/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Directorio directorio = db.Directorio.Find(id);
            db.Directorio.Remove(directorio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        DirectorioSahuro.Models.DirectorioDBContext db1 = new DirectorioSahuro.Models.DirectorioDBContext();

        [ValidateInput(false)]
        public ActionResult GridView3Partial()
        {
            var model = db1.Directorio;
            return PartialView("_GridView3Partial", model.Select(a => new { ID = a.ID, NOMBRE = a.NOMBRE, FECHA_NACIMIENTO = a.FECHA_NACIMIENTO, TELEFONOS = a.TELEFONOS, FOTOGRAFIA = ("<img src='" + a.FOTOGRAFIA + "' alt='NO_IMAGE' height='60px' width='60px'/>") }).OrderBy(a => a.NOMBRE).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridView3PartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] DirectorioSahuro.Models.Directorio item)
        {
            var model = db1.Directorio;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridView3Partial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridView3PartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] DirectorioSahuro.Models.Directorio item)
        {
            var model = db1.Directorio;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID == item.ID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridView3Partial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridView3PartialDelete(System.Int32 ID)
        {
            var model = db1.Directorio;
            if (ID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID == ID);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridView3Partial", model.ToList());
        }
    }
}