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
        static void Main()
        {
            var integrity = new FileIntegrity();
            integrity.Create(FileIntegrity.FileName);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
