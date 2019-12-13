using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infoscreen_Verwaltung.classes;

namespace ScreenCoreApp.Classes
{
    public class GeneralFunctions // snipped from ISv5
    {
        /// <summary>
        /// Findet die kleinste Stundennummer des übergebenen Stundenplans heraus
        /// </summary>
        /// <param name="_stundenplan">Der Stundenplan aus welchem die kleinste Stunde herausgefunden werden soll.</param>
        /// <returns>Die kleinste Stunde des Stundenplans</returns>
        static public int KleinsteStunde(Structuren.StundenplanTag[] _stundenplan)
        {
            try
            {
                int min = 20;
                for (int i1 = 0; i1 < _stundenplan.Length; i1++)
                {
                    for (int i2 = 0; i2 < _stundenplan[i1].StundenDaten.Length; i2++)
                    {
                        if (min > _stundenplan[i1].StundenDaten[i2].Stunde) min = _stundenplan[i1].StundenDaten[i2].Stunde;
                    }
                }
                return min;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Findet die größte Stundennummer des übergebenen Stundenplans heraus
        /// </summary>
        /// <param name="_stundenplan">Der Stundenplan aus welchem die größte Stunde herausgefunden werden soll.</param>
        /// <returns>Die größte Stunde des Stundenplans</returns>
        static public int GrößteStunde(Structuren.StundenplanTag[] _stundenplan)
        {
            try
            {
                int max = 0;
                for (int i1 = 0; i1 < _stundenplan.Length; i1++)
                {
                    for (int i2 = 0; i2 < _stundenplan[i1].StundenDaten.Length; i2++)
                    {
                        if (max < _stundenplan[i1].StundenDaten[i2].Stunde) max = _stundenplan[i1].StundenDaten[i2].Stunde;
                    }
                }
                return max;
            }
            catch { return -1; }
        }

		static public string ConvertBBtoHTML(string BB_Code)
		{
			BB_Code = BB_Code.Replace("[br]", "<br/>");
			BB_Code = BB_Code.Replace("\n", "<br/>");
			BB_Code = BB_Code.Replace("[s]", "<del>");
			BB_Code = BB_Code.Replace("[/s]", "</del>");
			BB_Code = BB_Code.Replace("[ulist]", "<UL>");
			BB_Code = BB_Code.Replace("[/ulist]", "</UL>");
			BB_Code = BB_Code.Replace("[olist]", "<OL>");
			BB_Code = BB_Code.Replace("[/olist]", "</OL>");
			BB_Code = BB_Code.Replace("[*]", "<LI>");
			BB_Code = BB_Code.Replace("[b]", "<B>");
			BB_Code = BB_Code.Replace("[/b]", "</B>");
			BB_Code = BB_Code.Replace("[strong]", "<STRONG>");
			BB_Code = BB_Code.Replace("[/strong]", "</STRONG>");
			BB_Code = BB_Code.Replace("[u]", "<u>");
			BB_Code = BB_Code.Replace("[/u]", "</u>");
			BB_Code = BB_Code.Replace("[i]", "<i>");
			BB_Code = BB_Code.Replace("[/i]", "</i>");
			BB_Code = BB_Code.Replace("[em]", "<em>");
			BB_Code = BB_Code.Replace("[/em]", "</em>");
			BB_Code = BB_Code.Replace("[sup]", "<sup>");
			BB_Code = BB_Code.Replace("[/sup]", "</sup>");
			BB_Code = BB_Code.Replace("[sub]", "<sub>");
			BB_Code = BB_Code.Replace("[/sub]", "</sub>");
			BB_Code = BB_Code.Replace("[hr]", "<HR>");
			BB_Code = BB_Code.Replace("[strike]", "<STRIKE>");
			BB_Code = BB_Code.Replace("[/strike]", "</STRIKE>");
			BB_Code = BB_Code.Replace("[h1]", "<h1>");
			BB_Code = BB_Code.Replace("[/h1]", "</h1>");
			BB_Code = BB_Code.Replace("[h2]", "<h2>");
			BB_Code = BB_Code.Replace("[/h2]", "</h2>");
			BB_Code = BB_Code.Replace("[h3]", "<h3>");
			BB_Code = BB_Code.Replace("[/h3]", "</h3>");
			BB_Code = BB_Code.Replace("[url=", "<A HREF=");
			BB_Code = BB_Code.Replace("[/url]", "</A>");
			BB_Code = BB_Code.Replace("']", "'>");
			while (BB_Code.IndexOf("[size=") != -1)
			{
				string text = BB_Code.Substring(BB_Code.IndexOf("[size="));
				text = text.Substring(0, text.IndexOf("]"));
				string str = text.Substring(text.IndexOf('=') + 1);
				BB_Code = BB_Code.Replace("[size=" + str + "]", "<span style=\"font-size: " + str + "px;\">");
			}
			BB_Code = BB_Code.Replace("[/size]", "</span>");
			while (BB_Code.IndexOf("[color=") != -1)
			{
				string text2 = BB_Code.Substring(BB_Code.IndexOf("[color="));
				text2 = text2.Substring(0, text2.IndexOf("]"));
				string str2 = text2.Substring(text2.IndexOf('=') + 1);
				BB_Code = BB_Code.Replace("[color=" + str2 + "]", "<span style=\"color: " + str2 + ";\">");
			}
			BB_Code = BB_Code.Replace("[/color]", "</span>");
			return BB_Code;
		}
	}    
}
