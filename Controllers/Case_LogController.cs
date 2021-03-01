using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CovidAppV5.Models;
using PagedList;

namespace CovidAppV5.Controllers
{
    public class Case_LogController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Case_Log
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

            var caseLog = from s in db.Case_Log
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                caseLog = caseLog.Where(s => s.Name.Contains(searchString)
                                       || s.OrgNumber.Contains(searchString)
                                       || s.Division_District.Contains(searchString)
                                       || s.DateOfExposure.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    caseLog = caseLog.OrderByDescending(s => s.Name);
                    break;
                case "Org":
                    caseLog = caseLog.OrderBy(s => s.OrgNumber);
                    break;
                case "Org_desc":
                    caseLog = caseLog.OrderByDescending(s => s.OrgNumber);
                    break;
                default:
                    caseLog = caseLog.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(caseLog.ToPagedList(pageNumber, pageSize));
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
            Console.WriteLine("Testing print");
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
            var currentPath = "";
            var currentPathQuery = from item in db.Case_Log
                                       where item.ID == case_Log.ID
                                       select item.PathToFile;

            foreach(var q in currentPathQuery)
            {
                currentPath = q;
            }

            if (PostedFile != null)
            {
                string path = Server.MapPath("~/Case_Log_Docs/");
                string fileName = Path.GetFileName(PostedFile.FileName);
                case_Log.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
            }
            else
            {
                case_Log.PathToFile = currentPath;
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

            string path = Server.MapPath("~/Case_Log_Docs/" + case_Log.PathToFile);
            //If file is a .jpg
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

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
