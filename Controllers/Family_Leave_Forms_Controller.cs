using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        // GET: Family_Leave_Forms_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Family_Leave_Forms_/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase PostedFile)
        {
            try
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
                    PostedFile.SaveAs(path + fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Family_Leave_Forms_/Delete/5
        [HttpPost]
        public ActionResult Delete(string fileName)
        {
            try
            {
                string path = Server.MapPath("~/Family_Leave_Docs/");
                string fullPath = path + fileName;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                TempData["UserMessage"] = "Success!";
                return RedirectToAction("Index", "Family_Leave_Forms_");
            }
            catch
            {
                TempData["UserMessage"] = "Something went wrong... :-(";
                return RedirectToAction("Family_Leave_Forms_");
            }
        }
    }
}
