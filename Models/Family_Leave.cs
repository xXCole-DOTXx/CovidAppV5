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

    public partial class Family_Leave
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [Column("Division/District")]
        public string Division_District { get; set; }
        public string OrgNumber { get; set; }
        public bool QuarantineOrder { get; set; }
        public bool AdviseToSelfQuarantine { get; set; }
        public bool Symptoms { get; set; }
        public bool CaringForPerson { get; set; }
        public bool ChildCareUnavailable { get; set; }
        public bool SimilarConditions { get; set; }
        public Nullable<double> Annual { get; set; }
        public Nullable<double> PaidSick { get; set; }
        public Nullable<double> EmergencyPaidSick { get; set; }
        public Nullable<double> Unpaid { get; set; }
        public System.DateTime LeaveFrom { get; set; }
        public System.DateTime LeaveTo { get; set; }
        public string PathToFile { get; set; }

        [NotMapped]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.pdf|.doc)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}
