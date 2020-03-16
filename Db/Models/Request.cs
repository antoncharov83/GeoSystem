using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoSystem.Models
{
    public class Request
    {
        [Field(FieldType = typeof(Ext.Net.Hidden))]
        public int RequestID { get; set; }
        //[Field(FieldType = typeof(Ext.Net.Hidden))]
        //public int BrigadeID { get; set; }
        [Display(Name = "Название")]
        public string RequestName { get; set; }
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        [Display(Name = "Дата начала")]
        public DateTime Start { get; set; }
        [Display(Name = "Дата окончания")]
        public DateTime End { get; set; }
        [Display(Name = "Закрыто")]
        public bool IsDone { get; set; }
        [UIHint("Brigade")]
        public virtual Brigade Brigade { get; set; }
    }
}