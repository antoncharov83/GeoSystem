using Ext.Net.MVC;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeoSystem.Models
{
    [Model(Name = "Brigade")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public class Brigade
    {
        [Field(FieldType = typeof(Ext.Net.Hidden), Ignore = true)]
        [ModelField(IDProperty = true, UseNull = true)]
        public int BrigadeID { get; set; }
        
        [Required]
        [PresenceValidator]
        [Display(Name = "Название")]
        public string BrigadeName { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}