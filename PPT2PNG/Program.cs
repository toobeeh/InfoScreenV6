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
            if(args.Length < 1)
            {
                Console.Write("Error: Parameter 'operating directory' is missing!");
                return;
            }

            if (args[0] == "help")
            {
                string help = "~~~ PPT to PNG Converter ~~~\n \n Written by Tobias Scharsching for InfoScreen V6 Diploma Thesis 2019/20\n\n";
                help += "Parameter: operating directory\nSearches for ppt file in directory and converts each slide to a single png using the Office CMD interface";
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
                
                var app = new Powerpoint.Application();
                Powerpoint.Presentation ppt = app.Presentations.Open(args[0]+@"\Datei.pptx");

                Console.WriteLine("Starting Converting " + ppt.Path);

                for (int i= 0; i< ppt.Slides.Count; i++)
                {
                    ppt.Slides[i].Export(args[0] + @"\" + i, "PNG");
                }
            }

            


        }
    }
}
