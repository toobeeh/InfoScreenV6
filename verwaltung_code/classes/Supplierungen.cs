//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infoscreen_Verwaltung.classes
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supplierungen
    {
        public int AbteilungsID { get; set; }
        public string Klasse { get; set; }
        public string Datum { get; set; }
        public string Stunde { get; set; }
        public string ErsatzFach { get; set; }
        public string ErsatzLehrerKürzel { get; set; }
        public string StattLehrerKürzel { get; set; }
        public Nullable<bool> Entfällt { get; set; }
        public Nullable<int> ZiehtVor { get; set; }
    }
}