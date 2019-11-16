using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using Powerpoint = Microsoft.Office.Interop.PowerPoint;


namespace PowerpointConverterService
{
    public partial class Converter : ServiceBase
    {
        /* 
         If infoscreen folder is changed -> Edit here and re-install service
         ATTENTION: Account which is used in following installation prompt has to be licensed for MS Office!

         Command in Admin-CMD to install service: 
            >"path to installutil" "path to service exe"
         Command in Admin-CMD to uninstall service: 
            >"path to installutil" /u "path to service exe"

         Example path to installutil: 
         C:\Windows\system32>C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe
        */

        private const string directory = @"D:\infoscreen_publish\PPT2PNG";
        
        public Converter()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer pollingDirectoryTimer = new Timer()
            {
                Interval = 10000
            };

            pollingDirectoryTimer.Elapsed += new ElapsedEventHandler(CheckDirectoryTree);
            pollingDirectoryTimer.Start();
        }

        protected override void OnStop()
        {
        }

        private void CheckDirectoryTree(object sender, ElapsedEventArgs args)
        {

            
            // search for "Datei" ppt / pptx files in directory
            List<string> powerpointFiles = new List<string>();

            if (File.Exists(directory + @"\convert.txt"))
            {
                powerpointFiles = System.IO.File.ReadAllLines(directory + @"\convert.txt").ToList<string>();

                File.Delete(directory + @"\convert.txt");

                // convert all files 
                foreach (string file in powerpointFiles)
                {
                    ConvertPowerpoint(file);
                }
            }  
        }

        // searches for all powerpoints -> issue: powerpoint isnt deleted afert conversion, so all ppts are found
        // -> UNUSEABLE
        private List<string> GetDirectoryPowerpoints(string searchDirectory)
        {
            List<string> powerpoints = new List<string>();

            foreach (string file in Directory.GetFiles(searchDirectory))
            {
                if (Path.GetFileName(file) == "Datei.ppt" || Path.GetFileName(file) == "Datei.pptx") powerpoints.Add(file);
            }

            if (Directory.GetDirectories(searchDirectory).Length > 0)
            {
                foreach (string subSearchDirectory in Directory.GetDirectories(searchDirectory))
                {
                    powerpoints.AddRange(GetDirectoryPowerpoints(subSearchDirectory));
                }
            }

            return powerpoints;
        }

        private void ConvertPowerpoint(string file)
        {
            string log = "";
            log += "Starting conversion of " + file + "\n";

            Powerpoint.Application app;
            Powerpoint.Presentation ppt;
            app = new Powerpoint.Application();

            try
            {
                ppt = app.Presentations.Open(file, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            }
            catch(Exception e)
            {
                log += e;
                File.WriteAllText(Path.GetDirectoryName(file) + @"\log.txt", log);
                return;
            }

            for (int i = 1; i <= ppt.Slides.Count; i++)
            {
                ppt.Slides[i].Export(Path.GetDirectoryName(file) + @"\" + i + ".png", "PNG");
            }

            ppt.Close();
            //File.Delete(file);
            log += "Conversion of " + file + " completed";
            File.WriteAllText(Path.GetDirectoryName(file) + @"\log.txt", log);
        }
    }
}
