using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Infoscreen_Verwaltung.classes
{
    /// <summary>
    /// Eine Klasse zur Verarbeitung von PowerPoint Dateien.
    /// </summary>
    public class PowerPoint
    {
        /// <summary>
        /// Konvertiert die Übergebene PowerPoint Datei in einzelne Bilddateien.
        /// </summary>
        /// <param name="_pfad">Der Pfad in welchem sich die Datei befindet und wohin die Dateien gespeichert werden sollen.</param>
        /// <param name="_datei">Der Name der PowerPoint Datei</param>
        public static void ZuBild(string _pfad, string _datei)
        {
            File.AppendAllText(@"D:\infoscreen_publish\PPT2PNG\convert.txt", _pfad + "\n");
            File.Copy(@"D:\infoscreen_publish\Screen\presentations\standard.png", _pfad + "1.png");
        }
    }
}