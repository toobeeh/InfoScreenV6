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
            StreamWriter sr = new StreamWriter(Properties.Resources.PfadPowerPointTextDatei, true);
            sr.WriteLine(_pfad + _datei);
            sr.Flush();
            sr.Close();
            sr.Dispose();
            File.Copy(Properties.Resources.StandardBild, _pfad + "1.png");
        }
    }
}