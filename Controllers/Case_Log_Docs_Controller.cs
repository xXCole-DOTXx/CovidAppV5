using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidAppV5.Controllers
{
    public class Case_Log_Docs_Controller : Controller
    {
        // GET: Case_Log_Docs_
        public ActionResult Index()
        {
            string path = Server.MapPath("~/Case_Log_Docs/");
            string[] fileEntries = Directory.GetFiles(path);
            var docs = new List<string>();
            foreach (string fileName in fileEntries)
            {
                string result = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                docs.Add(result);
            }
            string[] arrayOfDocs = docs.ToArray();
            ViewData["arrayOfDocs"] = arrayOfDocs; //Must include the array using both methods
            return View(arrayOfDocs);
        }

        // GET: Case_Log_Docs_/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Case_Log_Docs_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Case_Log_Docs_/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Case_Log_Docs_/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Case_Log_Docs_/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Case_Log_Docs_/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Case_Log_Docs_/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
