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
using Rotativa;

namespace CovidAppV5.Controllers
{
    public class Family_LeaveController : Controller
    {
        private Covid19Entities db = new Covid19Entities();

        // GET: Family_Leave
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, string leaveDateFrom, string leaveDateTo)
        {
            System.Diagnostics.Debug.WriteLine("The date from was: " + leaveDateFrom);
            System.Diagnostics.Debug.WriteLine("The date to was: " + leaveDateTo);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgSortParm = sortOrder == "Org" ? "Org_desc" : "Org";
            ViewBag.DivSortParm = sortOrder == "Div" ? "Div_desc" : "Div";
            ViewBag.Page = page;
            DateTime lFrom, lTo;

            if (searchString != null)
            {
                if (page <= 1)
                {
                    page = 1;
                }
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var fLeave = from s in db.Family_Leave
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                fLeave = fLeave.Where(s => s.Name.Contains(searchString)
                                       || s.OrgNumber.Contains(searchString)
                                       || s.Division_District.Contains(searchString));
            }

            ViewBag.leaveDateFrom = leaveDateFrom;
            ViewBag.leaveDateTo = leaveDateTo;

            if (!String.IsNullOrEmpty(leaveDateFrom) && (!String.IsNullOrEmpty(leaveDateTo)))
            {
                lFrom = Convert.ToDateTime(leaveDateFrom);
                lTo = Convert.ToDateTime(leaveDateTo);
                fLeave = fLeave.Where(s => s.LeaveFrom >= lFrom && s.LeaveTo <= lTo);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    fLeave = fLeave.OrderByDescending(s => s.Name);
                    break;
                case "Org":
                    fLeave = fLeave.OrderBy(s => s.OrgNumber);
                    break;
                case "Org_desc":
                    fLeave = fLeave.OrderByDescending(s => s.OrgNumber);
                    break;
                case "Div":
                    fLeave = fLeave.OrderBy(s => s.Division_District);
                    break;
                case "Div_desc":
                    fLeave = fLeave.OrderByDescending(s => s.Division_District);
                    break;
                default:
                    fLeave = fLeave.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.resultCount = fLeave.Count();
            return View(fLeave.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult pdfIndex(string searchString, string currentFilter, string sortOrder, string sort, string leaveDateFrom, string leaveDateTo)
        {
            System.Diagnostics.Debug.WriteLine("The sort string was: " + searchString);
            System.Diagnostics.Debug.WriteLine("The search string was: " + sort);
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgSortParm = sortOrder == "Org" ? "Org_desc" : "Org";
            ViewBag.DivSortParm = sortOrder == "Div" ? "Div_desc" : "Div";
            DateTime lFrom, lTo;

            var fLeave = from s in db.Family_Leave
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                fLeave = fLeave.Where(s => s.Name.Contains(searchString)
                                       || s.OrgNumber.Contains(searchString)
                                       || s.Division_District.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(leaveDateFrom) && (!String.IsNullOrEmpty(leaveDateTo)))
            {
                lFrom = Convert.ToDateTime(leaveDateFrom);
                lTo = Convert.ToDateTime(leaveDateTo);
                fLeave = fLeave.Where(s => s.LeaveFrom >= lFrom && s.LeaveTo <= lTo);
            }

            switch (sort)
            {
                case "name_desc":
                    fLeave = fLeave.OrderByDescending(s => s.Name);
                    break;
                case "Org":
                    fLeave = fLeave.OrderBy(s => s.OrgNumber);
                    break;
                case "Org_desc":
                    fLeave = fLeave.OrderByDescending(s => s.OrgNumber);
                    break;
                case "Div":
                    fLeave = fLeave.OrderBy(s => s.Division_District);
                    break;
                case "Div_desc":
                    fLeave = fLeave.OrderByDescending(s => s.Division_District);
                    break;
                default:
                    fLeave = fLeave.OrderBy(s => s.Name);
                    break;
            }

            ViewBag.resultCount = fLeave.Count();
            return View(fLeave);
        }

        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("pdfIndex");
            return report;
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
            Console.WriteLine("Testing print");

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

            return View(family_Leave);
        }

        // GET: Family_Leave/Edit/5
        public ActionResult Edit(int? id, string searchString, string sort, int? page)
        {
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PageNum = page;

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
        public ActionResult Edit([Bind(Include = "ID,Name,Phone1,Phone2,Division_District,OrgNumber,QuarantineOrder,AdviseToSelfQuarantine,Symptoms,CaringForPerson,ChildCareUnavailable,SimilarConditions,Annual,PaidSick,EmergencyPaidSick,Unpaid,LeaveFrom,LeaveTo,PathToFile")] Family_Leave family_Leave, HttpPostedFileBase PostedFile, string searchString, string sortOrder, int? page)
        {

            var currentPath = "";
            var currentPathQuery = from item in db.Family_Leave
                                   where item.ID == family_Leave.ID
                                   select item.PathToFile;

            foreach (var q in currentPathQuery)
            {
                currentPath = q;
            }

            if (PostedFile != null)
            {
                string path = Server.MapPath("~/Family_Leave_Docs/");
                string fileName = Path.GetFileName(PostedFile.FileName);
                family_Leave.PathToFile = fileName;
                PostedFile.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
            else
            {
                family_Leave.PathToFile = currentPath;
            }

            if (ModelState.IsValid)
            {
                db.Entry(family_Leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { searchString = searchString, sortOrder = sortOrder, page = page });
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

            string path = Server.MapPath("~/Family_Leave_Docs/" + family_Leave.PathToFile);
            //If file is a .jpg
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

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
