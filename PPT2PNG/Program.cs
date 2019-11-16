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
                Console.Write("Parameter 'operating directory' is missing, setting working directory " + Directory.GetCurrentDirectory() + " as directory");
                args = new string[] { Directory.GetCurrentDirectory() };
            }

            if (args[0] == "help")
            {
                string help = "~~~ PPT to PNG Converter ~~~\n \n Written by Tobias Scharsching for InfoScreen V6 Diploma Thesis 2019/20\n\n";
                help += "Parameter: operating directory\nWaits for a convert.txt in specified folder. The convert.txt has to contain the path to the directory of the powerpoint which should be converted.";
                Console.WriteLine(help);    
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Error: Specified operating directory doesn't exist!");
                return;
            }

            else
            {
                Console.WriteLine("Spectating now on " +  args[0]);
                WaitForTextEvent(args[0]);
                
            }
        }


        static void WaitForTextEvent(string dir)
        {

            while (true)
            {
                Console.WriteLine("Searching " + dir + @"\convert.txt ...");

                if (File.Exists(dir + @"\convert.txt"))
                {
                    string[] lines = System.IO.File.ReadAllLines(dir + @"\convert.txt");

                    File.Delete(dir + @"\convert.txt");

                    foreach(string line in lines)
                    {
                        
                        ConvertPowerpoint(line);
                    }

                }

                Console.WriteLine("Sleeping...");
                System.Threading.Thread.Sleep(5000);
            }
            
        }

        static void ConvertPowerpoint(string path)
        {

            Console.WriteLine("Initiating Conversion...");
            
            Powerpoint.Application app;
            Powerpoint.Presentation ppt;
            app = new Powerpoint.Application();

            try
            {
                Console.WriteLine("Looking for Datei.pptx");
                ppt = app.Presentations.Open(path + @"\Datei.pptx", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            }
            catch
            {
                try
                {
                    Console.WriteLine("Looking for Datei.ppt");
                    ppt = app.Presentations.Open(path + @"\Datei.ppt", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                }
                catch
                {
                    Console.WriteLine("No valid PPT found or MS Office is not installed! 'Datei.ppt', 'Datei.pptx'");
                    return;
                }
            }

            Console.WriteLine("Starting Converting " + ppt.Path);

            for (int i = 1; i <= ppt.Slides.Count; i++)
            {
                ppt.Slides[i].Export(path + @"\" + i + ".png", "PNG");
            }

            Console.WriteLine("Slides successfully converted");
            ppt.Close();
        }
    }
}
