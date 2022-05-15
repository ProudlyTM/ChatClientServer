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

            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(form.textBoxClientIP.Text), int.Parse(form.textBoxClientPort.Text));
                client.Connect(ipEndPoint);
                MessageBox.Show("Connected to server\n\n" + form.textBoxClientIP.Text.ToString() + ":" + form.textBoxClientPort.Text.ToString(), "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Program.STW = new StreamWriter(client.GetStream());
                Program.STR = new StreamReader(client.GetStream());
                Program.STW.AutoFlush = true;
                form.backgroundWorker1.RunWorkerAsync();
                form.backgroundWorker2.WorkerSupportsCancellation = true;

                form.textBoxClientIP.Enabled = false;
                form.textBoxClientPort.Enabled = false;
                form.btnConnect.Enabled = false;
                form.btnDisconnect.Enabled = true;
                form.comboBoxSrvIPSel.Enabled = false;
                form.textBoxSrvPort.Enabled = false;
                form.btnSrvStart.Enabled = false;
                form.btnSrvStop.Enabled = false;
                form.btnGenRandomPort.Enabled = false;
                //TODO: Switch above with form.EnableDisableControls when it's ready

                form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 20);
                form.lblStatus.ForeColor = Color.Green;
                form.lblStatus.Text = "Connected to\n" + form.textBoxClientIP.Text + ":" + form.textBoxClientPort.Text;
            }

            catch
            {
                MessageBox.Show("Invalid IP address specified\n\nCould not connect to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
