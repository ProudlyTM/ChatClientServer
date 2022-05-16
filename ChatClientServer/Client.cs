using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ChatClientServer
{
    internal class Client
    {
        private static Form1 form = Form1.Self;

        public static TcpClient client;

        public static void ConnectToSrv()
        {
            client = new TcpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(form.textBoxClientIP.Text), int.Parse(form.textBoxClientPort.Text));

            try
            {
                client.Connect(ipEndPoint);

                Program.STW = new StreamWriter(client.GetStream());
                Program.STR = new StreamReader(client.GetStream());
                Program.STW.AutoFlush = true;
                form.backgroundWorker1.RunWorkerAsync();
                form.backgroundWorker2.WorkerSupportsCancellation = true;

                form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 20);
                form.lblStatus.ForeColor = Color.Green;
                form.lblStatus.Text = "Connected to\n" + form.textBoxClientIP.Text + ":" + form.textBoxClientPort.Text;

                form.EnableDisableControls(true, false);

                MessageBox.Show("Connected to\n" + form.textBoxClientIP.Text + ":" + form.textBoxClientPort.Text, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch
            {
                MessageBox.Show("Invalid IP address specified\n\nCould not connect to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
