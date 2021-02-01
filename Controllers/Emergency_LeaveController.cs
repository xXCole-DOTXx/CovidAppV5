using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CovidAppV5.Models;
using PagedList;

namespace CovidAppV5.Controllers
{
    public class Emergency_LeaveController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Emergency_Leave
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgSortParm = sortOrder == "Org" ? "Org_desc" : "Org";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var eLeave = from s in db.Emergency_Leave
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                eLeave = eLeave.Where(s => s.Name.Contains(searchString)
                                       || s.OrgNumber.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    eLeave = eLeave.OrderByDescending(s => s.Name);
                    break;
                case "Org":
                    eLeave = eLeave.OrderBy(s => s.OrgNumber);
                    break;
                case "Org_desc":
                    eLeave = eLeave.OrderByDescending(s => s.OrgNumber);
                    break;
                default:
                    eLeave = eLeave.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(eLeave.ToPagedList(pageNumber, pageSize));
        }

        // GET: Emergency_Leave/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emergency_Leave emergency_Leave = db.Emergency_Leave.Find(id);
            if (emergency_Leave == null)
            {
                return HttpNotFound();
            }
            return View(emergency_Leave);
        }

        // GET: Emergency_Leave/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emergency_Leave/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,UnableToTelework,CaringForMinor,SpecialCircumstance,Annual,PaidSick,EmergencyPaidSick,Unpaid,LeaveFrom,LeaveTo,PathToFile")] Emergency_Leave emergency_Leave, HttpPostedFileBase PostedFile)
        {
            string path = Server.MapPath("~/Emergency_Leave_Docs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                System.Diagnostics.Debug.WriteLine("Created the folder.");
            }

            if (PostedFile != null)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                emergency_Leave.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            if (ModelState.IsValid)
            {
                db.Emergency_Leave.Add(emergency_Leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emergency_Leave);
        }

        // GET: Emergency_Leave/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emergency_Leave emergency_Leave = db.Emergency_Leave.Find(id);
            if (emergency_Leave == null)
            {
                return HttpNotFound();
            }
            return View(emergency_Leave);
        }

        // POST: Emergency_Leave/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,UnableToTelework,CaringForMinor,SpecialCircumstance,Annual,PaidSick,EmergencyPaidSick,Unpaid,LeaveFrom,LeaveTo,PathToFile")] Emergency_Leave emergency_Leave, HttpPostedFileBase PostedFile)
        {
            if (PostedFile != null)
            {
                string path = Server.MapPath("~/Emergency_Leave_Docs/");
                string fileName = Path.GetFileName(PostedFile.FileName);
                emergency_Leave.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            if (ModelState.IsValid)
            {
                db.Entry(emergency_Leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emergency_Leave);
        }

        // GET: Emergency_Leave/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emergency_Leave emergency_Leave = db.Emergency_Leave.Find(id);
            if (emergency_Leave == null)
            {
                return HttpNotFound();
            }
            return View(emergency_Leave);
        }

        // POST: Emergency_Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emergency_Leave emergency_Leave = db.Emergency_Leave.Find(id);
            db.Emergency_Leave.Remove(emergency_Leave);
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
