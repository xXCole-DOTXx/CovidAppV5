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
    public class Case_LogController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Case_Log
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Phone1SortParm = sortOrder == "Phone1" ? "phone1_desc" : "Phone1";
            var caseLog = from s in db.Case_Log
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                caseLog = caseLog.Where(s => s.Name.Contains(searchString)
                                       || s.Phone1.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    caseLog = caseLog.OrderByDescending(s => s.Name);
                    break;
                case "Phone1":
                    caseLog = caseLog.OrderBy(s => s.Phone1);
                    break;
                case "phone1_desc":
                    caseLog = caseLog.OrderByDescending(s => s.Phone1);
                    break;
                default:
                    caseLog = caseLog.OrderBy(s => s.Name);
                    break;
            }
            return View(caseLog.ToList());
        }

        // GET: Case_Log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case_Log case_Log = db.Case_Log.Find(id);
            if (case_Log == null)
            {
                return HttpNotFound();
            }
            return View(case_Log);
        }

        // GET: Case_Log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Case_Log/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,DateOfTest,DateOfExposure,NumberOfExposed,Notes,PathToFile")] Case_Log case_Log, HttpPostedFileBase PostedFile)
        {
            string path = Server.MapPath("~/Case_Log_Docs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                System.Diagnostics.Debug.WriteLine("Created the folder.");
            }

            if (PostedFile != null)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                case_Log.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }


            if (ModelState.IsValid)
            {
                db.Case_Log.Add(case_Log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(case_Log);
        }

        // GET: Case_Log/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case_Log case_Log = db.Case_Log.Find(id);
            if (case_Log == null)
            {
                return HttpNotFound();
            }
            return View(case_Log);
        }

        // POST: Case_Log/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,DateOfTest,DateOfExposure,NumberOfExposed,Notes,PathToFile")] Case_Log case_Log, HttpPostedFileBase PostedFile)
        {
            if (PostedFile != null)
            {
                string path = Server.MapPath("~/Case_Log_Docs/");
                string fileName = Path.GetFileName(PostedFile.FileName);
                case_Log.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            if (ModelState.IsValid)
            {
                db.Entry(case_Log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(case_Log);
        }

        // GET: Case_Log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case_Log case_Log = db.Case_Log.Find(id);
            if (case_Log == null)
            {
                return HttpNotFound();
            }
            return View(case_Log);
        }

        // POST: Case_Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case_Log case_Log = db.Case_Log.Find(id);
            db.Case_Log.Remove(case_Log);
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
