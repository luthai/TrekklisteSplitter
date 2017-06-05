using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TrekklisterSplitter
{
    static class Program
    {
        /// <summary>
        /// TrekklisterSplitter for å splitte pdf filen 
        /// til ulike fagforeninger og kreditorer
        /// Laget av Luan Thanh Thai
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMainWindow());
        }
    }
}

