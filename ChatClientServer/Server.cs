using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ChatClientServer
{
    internal class Server
    {
        private static Form1 form = Form1.Self;

        public static void StartSrv()
        {
            if (form.textBoxSrvPort.Text != "" && !(Int32.Parse(Program.srvPort) >= 65536))
            {
                TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(Program.srvPort));

                try
                {
                    listener.Start();

                    Thread srvThread = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        Client.client = listener.AcceptTcpClient();
                        Program.STR = new StreamReader(Client.client.GetStream());
                        Program.STW = new StreamWriter(Client.client.GetStream()) { AutoFlush = true };
                        form.backgroundWorker1.ReportProgress(100);
                        form.backgroundWorker1.RunWorkerAsync();
                        form.backgroundWorker2.WorkerSupportsCancellation = true;
                    });

                    srvThread.Start();

                    form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 20);
                    form.lblStatus.ForeColor = Color.Green;
                    form.lblStatus.Text = "Server running on port:\n" + Program.srvPort;

                    form.EnableDisableControls(true, true);

                    MessageBox.Show("Server started on port: " + Program.srvPort + "\nYou can use your IP from a common subnet in your network to connect", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex is SocketException)
                    {
                        MessageBox.Show("Entered port is already in use!\n\nTry again with another port", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (form.textBoxSrvPort.Text == "")
                {
                    MessageBox.Show("Server port can not be empty!\n\nMake sure to enter a port, which is not exceeding 65535.\n\n" +
                        "Or click the button below to auto-fill a random unused port", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Entered port is out of range!\n\nMake sure the port is not exceeding 65535", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void GetLocalIPs()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    form.comboBoxSrvIPSel.Items.Add(ip);
                }
            }
        }

        public static int GetRandomUnusedPort()
        {
            var portListener = new TcpListener(IPAddress.Any, 0);
            portListener.Start();
            var port = ((IPEndPoint)portListener.LocalEndpoint).Port;
            portListener.Stop();
            return port;
        }
    }
}
