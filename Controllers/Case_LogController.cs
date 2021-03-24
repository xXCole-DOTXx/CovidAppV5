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
using Rotativa;

namespace CovidAppV5.Controllers
{
    public class Case_LogController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Case_Log
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string ExpDateFrom, string ExpDateTo)
        {
            System.Diagnostics.Debug.WriteLine("The date from was: " + ExpDateFrom);
            System.Diagnostics.Debug.WriteLine("The date to was: " + ExpDateTo);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgSortParm = sortOrder == "Org" ? "Org_desc" : "Org";
            ViewBag.DivSortParm = sortOrder == "Div" ? "Div_desc" : "Div";
            DateTime fromDate, toDate;

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
                                       || s.Division_District.Contains(searchString));
            }

            ViewBag.ExpDateFrom = ExpDateFrom;
            ViewBag.ExpDateTo = ExpDateTo;

            if (!String.IsNullOrEmpty(ExpDateFrom) && (!String.IsNullOrEmpty(ExpDateTo)))
            {
                fromDate = Convert.ToDateTime(ExpDateFrom);
                toDate = Convert.ToDateTime(ExpDateTo);
                caseLog = caseLog.Where(s => s.DateOfExposure >= fromDate && s.DateOfExposure <= toDate);
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
                case "Div":
                    caseLog = caseLog.OrderBy(s => s.Division_District);
                    break;
                case "Div_desc":
                    caseLog = caseLog.OrderByDescending(s => s.Division_District);
                    break;
                default:
                    caseLog = caseLog.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.resultCount = caseLog.Count();
            return View(caseLog.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult pdfIndex(string searchString, string sortOrder, string sort, string expDateFrom, string expDateTo)
        {
            System.Diagnostics.Debug.WriteLine("The sort string was: " + searchString);
            System.Diagnostics.Debug.WriteLine("The search string was: " + sort);
            System.Diagnostics.Debug.WriteLine("The date from was: " + expDateFrom);
            System.Diagnostics.Debug.WriteLine("The date to was: " + expDateTo);
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgSortParm = sortOrder == "Org" ? "Org_desc" : "Org";
            ViewBag.DivSortParm = sortOrder == "Div" ? "Div_desc" : "Div";
            //ViewBag.ExpDateFrom = expDateFrom;
            //ViewBag.ExpDateTo = expDateTo;
            DateTime fromDate, toDate;

            var caseLog = from s in db.Case_Log
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                caseLog = caseLog.Where(s => s.Name.Contains(searchString)
                                       || s.OrgNumber.Contains(searchString)
                                       || s.Division_District.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(expDateFrom) && (!String.IsNullOrEmpty(expDateTo)))
            {
                fromDate = Convert.ToDateTime(expDateFrom);
                toDate = Convert.ToDateTime(expDateTo);
                System.Diagnostics.Debug.WriteLine("From DateTime: " + fromDate);
                System.Diagnostics.Debug.WriteLine("To DateTime: " + toDate);

                caseLog = caseLog.Where(s => s.DateOfExposure >= fromDate && s.DateOfExposure <= toDate);
            }

            switch (sort)
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
                case "Div":
                    caseLog = caseLog.OrderBy(s => s.Division_District);
                    break;
                case "Div_desc":
                    caseLog = caseLog.OrderByDescending(s => s.Division_District);
                    break;
                default:
                    caseLog = caseLog.OrderBy(s => s.Name);
                    break;
            }

            ViewBag.resultCount = caseLog.Count();
            return View(caseLog);
        }

        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("pdfIndex");
            return report;
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
                System.Diagnostics.Debug.WriteLine("Filename: " + fileName);
                case_Log.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            if (ModelState.IsValid)
            {
                db.Case_Log.Add(case_Log);
                try
                {
                    db.SaveChanges();
                }//This catch is how you figure out what the EntityValidationErrors are
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting  
                            // the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

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
