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

            string running_info = "";

            if (File.Exists(Directory.GetCurrentDirectory() + @"\run.txt"))
            {
                running_info = File.ReadAllText(Directory.GetCurrentDirectory() + @"\run.txt");
            }

            List<string> lines = running_info.Split('\n').ToList<string>();

            if(lines.Count > 1 )
            {
                DateTime lastsync = DateTime.ParseExact(lines[0], "MM/dd/yyyy HH:mm:ss", null);
                DateTime now = DateTime.Now;
                double secs = (now - lastsync).TotalSeconds;

                if (secs < 10)
                {
                    Console.WriteLine("PPT Converter is already running on user " + lines[1] + ". \n Press any Key to exit.");
                    Console.ReadKey();
                    return;
                }  
            }
           
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\run.txt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\n" + Environment.UserName);

            if (args.Length < 1)
            {
                Console.Write("Parameter 'operating directory' is missing, setting working directory " + Directory.GetCurrentDirectory() + " as directory \n");
                args = new string[] { Directory.GetCurrentDirectory() };
            }

            if (args[0] == "help")
            {
                string help = "~~~ PPT to PNG Converter ~~~\n \n Written by Tobias Scharsching for InfoScreen V6 Diploma Thesis 2019/20\n\n";
                help += "Parameter: operating directory\nWaits for a convert.txt in specified folder. The convert.txt has to contain the path to the directory of the powerpoint which should be converted.";
                help += "\nThe converter service can only run if any user is signed up on the infoscreen server. Otherwise, powerpoints wont be automatically converted until a user logs in.";
                Console.WriteLine(help);    
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Error: Specified operating directory doesn't exist!");
                File.Delete(Directory.GetCurrentDirectory() + @"\run.txt");
                return;
            }

            else
            {
                WaitForTextEvent(args[0]);
            }
        }

        static void WaitForTextEvent(string dir)
        {
            

            while (true)
            {
                Console.WriteLine("Searching " + dir + @"\convert.txt ...");
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\run.txt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\n" + Environment.UserName);

                if (File.Exists(dir + @"\convert.txt"))
                {
                    string[] lines = System.IO.File.ReadAllLines(dir + @"\convert.txt");
                    File.Delete(dir + @"\convert.txt");

                    foreach(string line in lines)
                    {
                        File.WriteAllText(Directory.GetCurrentDirectory() + @"\run.txt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\n" + Environment.UserName);
                        if (Directory.Exists(line)) ConvertPowerpoint(line);
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
