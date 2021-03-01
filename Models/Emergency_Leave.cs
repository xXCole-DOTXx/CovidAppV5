//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CovidAppV5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Emergency_Leave
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Division_District { get; set; }
        public string OrgNumber { get; set; }
        public bool UnableToTelework { get; set; }
        public bool CaringForMinor { get; set; }
        public bool SpecialCircumstance { get; set; }
        public double Annual { get; set; }
        public double PaidSick { get; set; }
        public double EmergencyPaidSick { get; set; }
        public double Unpaid { get; set; }
        public Nullable<System.DateTime> LeaveFrom { get; set; }
        public Nullable<System.DateTime> LeaveTo { get; set; }
        public string PathToFile { get; set; }
        [NotMapped]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.pdf|.doc)$", ErrorMessage = "That file type is no allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}

