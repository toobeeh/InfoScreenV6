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

namespace PPTConverterService
{
    public partial class PPTConverter : ServiceBase
    {
        public PPTConverter()
        {
            InitializeComponent();
        }

        private string directory;

        protected override void OnStart(string[] args)
        {
            //eventLog.WriteEntry("PowerPoint to PNG Converter has started and will supervise the infoscreen presentation directories D:/infoscreen-publish/Screen/presentations");
            directory = @"C:\Users\Tobi\Desktop\ppt";

            Timer pollingDirectoryTimer = new Timer()
            {
                Interval = 100000
            };

            pollingDirectoryTimer.Elapsed += new ElapsedEventHandler(this.CheckDirectoryTree);
            pollingDirectoryTimer.Start();

        }

        protected override void OnStop()
        {
            //eventLog.WriteEntry("PowerPoint to PNG Converter has been stopped.");
        }

        private void CheckDirectoryTree(object sender, ElapsedEventArgs args)
        {
            // search for "Datei" ppt / pptx files in directory
            List<string> powerpointFiles = GetDirectoryPowerpoints(directory);

            // convert all files 
            foreach (string file in powerpointFiles)
            {
                ConvertPowerpoint(file);
            }
        }

        private List<string> GetDirectoryPowerpoints(string searchDirectory)
        {
            List<string> powerpoints = new List<string>();

            foreach (string file in Directory.GetFiles(searchDirectory))
            {
                if (Path.GetFileName(file) == "Datei.ppt" || Path.GetFileName(file) == "Datei.pptx") powerpoints.Add(file);
            }

            if(Directory.GetDirectories(searchDirectory).Length > 0)
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
            //EventLog.WriteEntry("Initiating Conversion of " + file);
            Powerpoint.Application app;
            Powerpoint.Presentation ppt;
            app = new Powerpoint.Application();

            try
            {
                //EventLog.WriteEntry("Looking for Datei.ppt");
                ppt = app.Presentations.Open(file, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            }
            catch
            {
                //EventLog.WriteEntry("PPT not valid or MS Office is not installed!");
                return ;
            }
            
            //EventLog.WriteEntry("Converting " + ppt.Path);

            for (int i = 1; i <= ppt.Slides.Count; i++)
            {
                ppt.Slides[i].Export(Path.GetDirectoryName(file) + @"\" + i + ".png", "PNG");
            }

            //EventLog.WriteEntry("Slides successfully converted");
            ppt.Close();
            //File.Delete(file);
        }

    }
}
