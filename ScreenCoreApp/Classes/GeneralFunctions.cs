using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infoscreen_Bibliotheken;

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
    }    
}
