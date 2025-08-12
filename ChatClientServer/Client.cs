using System;
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

        private static bool isIntentionalDisconnect = false;

        public static bool IsIntentionalDisconnect()
        {
            return isIntentionalDisconnect;
        }

        public static async void ConnectToSrv()
        {
            try
            {
                form.btnConnect.Enabled = false;
                form.btnConnect.Font = new Font(form.btnConnect.Font.Name, 8);
                form.btnConnect.Text = "CONNECTING...";
                form.EnableDisableControls(true, false);
                form.btnDisconnect.Enabled = false;

                client = new TcpClient();

                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(form.textBoxClientIP.Text), int.Parse(form.textBoxClientPort.Text));
                await client.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);

                Program.STW = new StreamWriter(client.GetStream());
                Program.STR = new StreamReader(client.GetStream());
                Program.STW.AutoFlush = true;
                form.backgroundWorker1.RunWorkerAsync();

                form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 20);
                form.lblStatus.ForeColor = Color.Green;

                form.btnConnect.Font = new Font(form.btnConnect.Font.Name, 12);
                form.btnConnect.Text = "CONNECT";

                bool isServerSelfConnection = (form.textBoxClientIP.Text == "127.0.0.1" &&
                                             form.textBoxSrvPort.Text == form.textBoxClientPort.Text);

                if (isServerSelfConnection && Server.isSrvRunning)
                {
                    form.EnableDisableControls(true, true);
                }
                else
                {
                    form.lblStatus.Text = "Connected to\n" + form.textBoxClientIP.Text + ":" + form.textBoxClientPort.Text;
                    form.EnableDisableControls(true, false);
                }

                MessageBox.Show("Connected to\n" + form.textBoxClientIP.Text + ":" + form.textBoxClientPort.Text, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                form.tabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                form.btnConnect.Enabled = true;
                form.btnConnect.Font = new Font(form.btnConnect.Font.Name, 12);
                form.btnConnect.Text = "CONNECT";
                form.EnableDisableControls(false, false);

                string errorMessage = "Could not connect to server";
                if (ex is FormatException)
                    errorMessage = "Invalid IP address format";
                else if (ex is SocketException)
                    errorMessage = "Connection failed - server may be offline or unreachable";

                MessageBox.Show(errorMessage + "\n\n" + ex.Message, "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async void DisconnectFromSrv()
        {
            try
            {
                isIntentionalDisconnect = true;

                form.btnDisconnect.Enabled = false;
                form.btnDisconnect.Font = new Font(form.btnDisconnect.Font.Name, 8);
                form.btnDisconnect.Text = "DISCONNECTING...";
                form.EnableDisableControls(true, false);
                form.btnConnect.Enabled = false;

                if (form.backgroundWorker1.IsBusy)
                {
                    form.backgroundWorker1.CancelAsync();
                }
                if (form.backgroundWorker2.IsBusy)
                {
                    form.backgroundWorker2.CancelAsync();
                }

                if (client != null && client.Connected)
                {
                    try
                    {
                        Program.STW?.Close();
                        Program.STR?.Close();
                        client.Close();
                        client = null;
                    }
                    catch { }
                }

                await System.Threading.Tasks.Task.Delay(100);

                form.btnDisconnect.Enabled = true;
                form.btnDisconnect.Font = new Font(form.btnDisconnect.Font.Name, 12);
                form.btnDisconnect.Text = "DISCONNECT";
                form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 28);
                form.lblStatus.ForeColor = Color.Black;
                form.lblStatus.Text = "OR";

                form.EnableDisableControls(false, false);
                form.Text = "Chat Client/Server";

                MessageBox.Show("Disconnected from server", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await System.Threading.Tasks.Task.Delay(50);
                isIntentionalDisconnect = false;
            }
            catch (Exception ex)
            {
                isIntentionalDisconnect = false;

                form.btnDisconnect.Font = new Font(form.btnDisconnect.Font.Name, 12);
                form.btnDisconnect.Text = "DISCONNECT";
                form.EnableDisableControls(false, false);

                MessageBox.Show("Error disconnecting: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}