using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoSystem.Models
{
    public class Brigade
    {
        [Field(FieldType = typeof(Ext.Net.Hidden))]
        public int BrigadeID { get; set; }
        
        [Required]
        [Display(Name = "Название")]
        public string BrigadeName { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}