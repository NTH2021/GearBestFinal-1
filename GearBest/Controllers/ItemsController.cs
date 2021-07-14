using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GearBest.Models;

namespace GearBest.Controllers
{
    public class ItemsController : Controller
    {
        private GearBestEntities db = new GearBestEntities();

        // GET: Items
        public ActionResult Index()
        {
			var items = db.Items.Include(i => i.Brand).Include(i => i.ItemType).Where(a=>a.Active==true);
            return View(items.ToList());
        }
		public ActionResult Itemunactive()
		{
			var items = db.Items.Include(i => i.Brand).Include(i => i.ItemType).Where(a => a.Active == false);
			return View(items.ToList());
		}

		// GET: Items/Details/5
		public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name");
            ViewBag.TypeID = new SelectList(db.ItemTypes, "ID", "TypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create([Bind(Include = "ID,Name,PurcharsePrice,SellPrice,Quantity,TypeID,BrandID,Picture,ShortTitle,Describe")] Item item)
        {
            if (ModelState.IsValid)
            {
				item.DateImport = DateTime.Now;
				item.Active = true;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", item.BrandID);
            ViewBag.TypeID = new SelectList(db.ItemTypes, "ID", "TypeName", item.TypeID);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", item.BrandID);
            ViewBag.TypeID = new SelectList(db.ItemTypes, "ID", "TypeName", item.TypeID);
            return View(item);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit([Bind(Include = "ID,Name,PurcharsePrice,SellPrice,DateImport,Quantity,TypeID,BrandID,Picture,ShortTitle,Describe")] Item item)
        {
            if (ModelState.IsValid)
            {
				
				ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", item.BrandID);
				db.Entry(item).State = EntityState.Modified;
				item.Active = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", item.BrandID);
            ViewBag.TypeID = new SelectList(db.ItemTypes, "ID", "TypeName", item.TypeID);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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

		public ActionResult UnactiveProduct(long? id)
		{
		
			var temp = db.Items.SingleOrDefault(p => p.ID == id);
			temp.Active = false;
			db.SaveChanges();

			return RedirectToAction("Index");
		}
		public ActionResult Active(long? id)
		{

			var temp = db.Items.SingleOrDefault(p => p.ID == id);
			temp.Active = true;
			db.SaveChanges();

			return RedirectToAction("Itemunactive");
		}
	}
}
