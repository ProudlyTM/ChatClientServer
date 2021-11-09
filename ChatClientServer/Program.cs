using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ChatClientServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            string thisProcessName = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1)
            {
                MessageBox.Show("Chat Client/Server is already running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}