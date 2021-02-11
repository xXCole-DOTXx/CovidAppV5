using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CovidAppV5.ViewModel
{
    public class COVIDvm
    {
        //All from questionaires

        public string Name { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [Column("Division/District")]
        public string Division_District { get; set; }
        public string OrgNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime DateOfTest { get; set; }
        public string DateOfExposure { get; set; }

        //All from Emergency Leave
        public bool UnableToTelework { get; set; }
        public bool CaringForMinor { get; set; }
        public bool SpecialCircumstance { get; set; }
        public double Annual { get; set; }
        public double PaidSick { get; set; }
        public double EmergencyPaidSick { get; set; }
        public double Unpaid { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime LeaveFrom { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime LeaveTo { get; set; }

        //All from Family Leave
        public bool QuarantineOrder { get; set; }
        public bool AdviseToSelfQuarantine { get; set; }
        public bool Symptoms { get; set; }
        public bool CaringForPerson { get; set; }
        public bool ChildCareUnavailable { get; set; }
        public bool SimilarConditions { get; set; }
    }
}