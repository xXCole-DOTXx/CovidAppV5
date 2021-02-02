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
        public ActionResult Index(string currentFilter, string searchString)
        {
            string path = Server.MapPath("~/Case_Log_Docs/");
            string[] fileEntries = Directory.GetFiles(path);
            var docs = new List<string>();

            if (!String.IsNullOrEmpty(searchString)) //If there is a search string
            {
                foreach (string fileName in fileEntries)
                {
                    if (fileName.Contains(searchString))
                    {
                        string result = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                        docs.Add(result);
                    }
                }
                string[] arrayOfDocs = docs.ToArray();
                ViewData["arrayOfDocs"] = arrayOfDocs; //Must include the array using both methods
                return View(arrayOfDocs);
            }
            else //If there is not a search string
            {
                foreach (string fileName in fileEntries)
                {
                    string result = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                    docs.Add(result);
                }
                string[] arrayOfDocs = docs.ToArray();
                ViewData["arrayOfDocs"] = arrayOfDocs; //Must include the array using both methods
                return View(arrayOfDocs);
            } 
        }

        // GET: Case_Log_Docs_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Case_Log_Docs_/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase PostedFile)
        {
            try
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

        // POST: Case_Log_Docs_/Delete/5
        [HttpPost]
        public ActionResult Delete(string fileName)
        {
            try
            {
                string path = Server.MapPath("~/Case_Log_Docs/");
                string fullPath = path + fileName;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                TempData["UserMessage"] = "Success!";
                return RedirectToAction("Index", "Case_Log_Docs_");
            }
            catch
            {
                TempData["UserMessage"] = "Something went wrong... :-(";
                return RedirectToAction("Case_Log_Docs_");
            }
        }
    }
}
