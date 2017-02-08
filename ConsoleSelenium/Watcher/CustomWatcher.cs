using ConsoleSelenium.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleSelenium.Watcher
{
    class CustomWatcher
    {
        public FileSystemWatcher fsWatch { get; set; }
        public static int numPage { get; set; }
        public static string fmt { get; set; }
        public static List<string> files { get; set; }

        public CustomWatcher()
        {
            fsWatch = new FileSystemWatcher(Const.dlFolder, "*.pdf");
            fsWatch.NotifyFilter = NotifyFilters.LastWrite;

            //Add event handlers.
            fsWatch.Changed += new FileSystemEventHandler(OnCreated);
            fsWatch.Created += new FileSystemEventHandler(OnCreated);

            //Start monitoring.
            fsWatch.EnableRaisingEvents = true;

            numPage = 0;
            fmt = "000";
            files = new List<string>();

        }

        public static void OnCreated(object source, FileSystemEventArgs e)
        {
            //Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);

            string newName = Const.dlFolder + Const.bSlash + Const.fileName + numPage.ToString(fmt) + ".pdf";
            files.Add(newName);

            File.Move(e.FullPath, newName);
            numPage++;

        }
    }
}
