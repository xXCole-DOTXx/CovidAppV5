using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidAppV5.Controllers
{
    public class Family_Leave_Forms_Controller : Controller
    {
        // GET: Family_Leave_Forms_
        public ActionResult Index()
        {
            string path = Server.MapPath("~/Family_Leave_Docs/");
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {
                System.Diagnostics.Debug.WriteLine(fileName);
            }
            ViewData["fileEntries"] = fileEntries;
            return View(fileEntries);
        }

        // GET: Family_Leave_Forms_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Family_Leave_Forms_/Create
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

        // GET: Family_Leave_Forms_/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Family_Leave_Forms_/Delete/5
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
