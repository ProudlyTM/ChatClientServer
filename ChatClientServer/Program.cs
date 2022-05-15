using System;
using System.IO;
using System.Windows.Forms;

namespace ChatClientServer
{
    static class Program
    {
        public static StreamReader STR;
        public static StreamWriter STW;

        public static string srvPort, receive, textToSend, encryptedText, decryptedText, decryptedBuffer;

        public static string chatHistory = Directory.GetCurrentDirectory() + "\\history.log";
        public static string[] chatHistoryLines;

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