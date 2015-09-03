using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using IntegrityChecker;
using IntegrityChecker.Data;

namespace IntegrityChecker
{
    public static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var commander = new CommandLineManager();
            commander.CommandList.Add("test", Test);
            commander.CommandList.Add("r", Read);
            commander.CommandList.Add("read", Read);

            commander.Parse(args);

            if (commander.IsCommandLineEmpty)
            {
                var integrity = new FileIntegrity();
                integrity.Create(FileIntegrity.FileName);
            }

            //var integrity = new FileIntegrity();
            ////integrity.Create(FileIntegrity.FileName);
            //integrity.Read(FileIntegrity.FileName);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        static void Read(string[] parameters)
        {
            if (parameters == null || parameters.Length < 1) throw new ArgumentNullException("Path is null or empty");
            var path = parameters[0];

            var integrity = new FileIntegrity();
            integrity.Read(path);
        }

        static void Test(string[] parameters)
        {
            MessageBox.Show("Test");
        }
    }
}
