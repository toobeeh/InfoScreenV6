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
    
    public partial class Betriebsanzeige
    {
        public int BildschirmID { get; set; }
        public int BetriebsmodeID { get; set; }
        public int DateiID { get; set; }
        public Nullable<bool> Stundenplan { get; set; }
        public Nullable<bool> Klasseninfo { get; set; }
        public Nullable<bool> Abteilungsinfo { get; set; }
        public Nullable<bool> Sprechstunden { get; set; }
        public Nullable<bool> Raumaufteilung { get; set; }
        public Nullable<bool> Supplierplan { get; set; }
        public Nullable<bool> AktuelleSupplierungen { get; set; }
    }
}