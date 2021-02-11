using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CovidAppV5.Models;
using CovidAppV5.ViewModel;

namespace CovidAppV5.Controllers
{
    public class ReportController : Controller
    {
        private Covid19Entities db = new Covid19Entities();
        // GET: Report
        public ActionResult Index()
        {
            List<COVIDvm> VMlist = new List<COVIDvm>(); // to hold list of forms
            var covidQuery = (from form in db.Case_Log
                              where form.Name == "Cole"
                              join eForm in db.Emergency_Leave on form.Name equals eForm.Name
                              select new {form.Name, form.Phone1, form.Phone2, eForm.OrgNumber, eForm.Annual, eForm.PaidSick}).ToList();
           // select form).ToList();

            foreach (var item in covidQuery)
            {
                COVIDvm objcvm = new COVIDvm(); // ViewModel
                objcvm.Name = item.Name;
                objcvm.Phone1 = item.Phone1;
                objcvm.Phone2 = item.Phone2;
                objcvm.OrgNumber = item.OrgNumber;
                objcvm.Division_District = item.Annual.ToString();
                objcvm.OrgNumber = item.PaidSick.ToString();
                VMlist.Add(objcvm);
            }
            return View(VMlist);
        }
    }
}
