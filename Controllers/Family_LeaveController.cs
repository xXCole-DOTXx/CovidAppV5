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

namespace CovidAppV5.Controllers
{
    public class Family_LeaveController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Family_Leave
        public ActionResult Index()
        {
            return View(db.Family_Leave.ToList());
        }

        // GET: Family_Leave/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family_Leave family_Leave = db.Family_Leave.Find(id);
            if (family_Leave == null)
            {
                return HttpNotFound();
            }
            return View(family_Leave);
        }

        // GET: Family_Leave/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Family_Leave/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,QuarantineOrder,AdviseToSelfQuarantine,Symptoms,CaringForPerson,ChildCareUnavailable,SimilarConditions,Annual,PaidSick,EmergencyPaidSick,Unpaid,LeaveFrom,LeaveTo,PathToFile")] Family_Leave family_Leave, HttpPostedFileBase PostedFile)
        {
            string path = Server.MapPath("~/Family_Leave_Docs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                System.Diagnostics.Debug.WriteLine("Created the folder.");
            }

            if (PostedFile != null)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                family_Leave.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            if (ModelState.IsValid)
            {
                db.Family_Leave.Add(family_Leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family_Leave);
        }

        // GET: Family_Leave/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family_Leave family_Leave = db.Family_Leave.Find(id);
            if (family_Leave == null)
            {
                return HttpNotFound();
            }
            return View(family_Leave);
        }

        // POST: Family_Leave/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,QuarantineOrder,AdviseToSelfQuarantine,Symptoms,CaringForPerson,ChildCareUnavailable,SimilarConditions,Annual,PaidSick,EmergencyPaidSick,Unpaid,LeaveFrom,LeaveTo,PathToFile")] Family_Leave family_Leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(family_Leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family_Leave);
        }

        // GET: Family_Leave/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family_Leave family_Leave = db.Family_Leave.Find(id);
            if (family_Leave == null)
            {
                return HttpNotFound();
            }
            return View(family_Leave);
        }

        // POST: Family_Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Family_Leave family_Leave = db.Family_Leave.Find(id);
            db.Family_Leave.Remove(family_Leave);
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
