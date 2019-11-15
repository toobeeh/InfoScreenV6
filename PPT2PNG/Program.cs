using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Powerpoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office;

namespace PPT2PNG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);


            if (args.Length < 1)
            {
                Console.Write("Error: Parameter 'operating directory' is missing!");
                return;
            }

            if (args[0] == "help")
            {
                string help = "~~~ PPT to PNG Converter ~~~\n \n Written by Tobias Scharsching for InfoScreen V6 Diploma Thesis 2019/20\n\n";
                help += "Parameter: operating directory\nSearches for pptx/ppt file in directory and converts each slide to a single png using the Office CMD interface";
                Console.WriteLine(help);    
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Error: Specified operating directory doesn't exist!");
                return;
            }

            else
            {
                Console.WriteLine(args[0]);

                Powerpoint.Application app;
                Powerpoint.Presentation ppt;
                try
                {
                    app = new Powerpoint.Application();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }

                try
                {
                    Console.WriteLine("Looking for Datei.pptx");
                    ppt = app.Presentations.Open(args[0] + @"\Datei.pptx", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                }
                catch
                {
                    try
                    {
                        Console.WriteLine("Looking for Datei.ppt");
                        ppt = app.Presentations.Open(args[0] + @"\Datei.ppt", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                    }
                    catch
                    {
                        Console.WriteLine("No valid PPT found or MS Office is not installed! 'Datei.ppt', 'Datei.pptx'");
                        return;
                    }
                }

                Console.WriteLine("Starting Converting " + ppt.Path);

                for (int i= 1; i<= ppt.Slides.Count; i++)
                {
                    ppt.Slides[i].Export(args[0] + @"\" + i+ ".png", "PNG");
                }

                Console.WriteLine("Slides successfully converted");
                ppt.Close();
            }
        }
    }
}
