using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class SchoolYear : DbModelBase
    {
        [Display(Name = "ปีการศึกษา")]
        public string Year { get; set; }
        [Display(Name = "วันเริ่มปีการศึกษา")]
        public DateTime StartDate { get; set; }
        public int SchoolId { get; set; }

        public string StartDateString { get { return StartDate.ToString(WebConfiguration.DateTimeFormat); } }
    }
}
