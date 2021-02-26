
namespace CovidAppV5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Case_Log
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Division_District { get; set; }
        public string OrgNumber { get; set; }
        public string DateOfTest { get; set; }
        public string DateOfExposure { get; set; }
        public string NumberOfExposed { get; set; }
        public string Notes { get; set; }
        public string PathToFile { get; set; }

        [NotMapped]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.pdf|.doc)$", ErrorMessage = "That file type is no allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}
