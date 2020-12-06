using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class DbModelBase
    {
        /// <summary>
        /// รหัส
        /// </summary>
        [Display(Name = "รหัส")]
        public int Id { get; set; }
        /// <summary>
        /// วันที่สร้างข้อมูล
        /// </summary>
        [Display(Name = "วันที่สร้างข้อมูล")]
        public DateTime CreatedDate { get; set; }

        public string CreatedDateString { get { return CreatedDate.ToString(WebConfiguration.DateTimeFormat); } }
    }
}
