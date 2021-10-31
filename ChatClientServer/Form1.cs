using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ChatClientServer
{
    public partial class Form1 : Form
    {
        private string srvPort;
        private string receive;
        private string textToSend;
        private StreamReader STR;
        private StreamWriter STW;
        private TcpClient client;

        public void GetLocalIPs()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    comboBoxSrvIPSel.Items.Add(ip);
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetLocalIPs();
            comboBoxSrvIPSel.SelectedIndex = 0;
            srvPort = GetRandomUnusedPort().ToString();
            lblSrvPort.Text += srvPort;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    this.textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                    {
                        textBoxChatScreen.AppendText("Person B: " + receive + "\n");
                    }));
                    receive = "";
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(textToSend);
                this.textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                {
                    textBoxChatScreen.AppendText("Person A: " + textToSend + "\n");
                }
                ));
            }
            else
            {
                MessageBox.Show("Sending Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            backgroundWorker2.CancelAsync();
        }

        private void btnSrvStart_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(srvPort));
                listener.Start();
                client = listener.AcceptTcpClient();
                STR = new StreamReader(client.GetStream());
                STW = new StreamWriter(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }).Start();

            btnConnect.Enabled = false;
            btnSrvStart.Enabled = false;
            textBoxClientIP.Enabled = false;
            textBoxClientPort.Enabled = false;
            MessageBox.Show("Server started on port: " + srvPort + "\nYou can use your IP from a common subnet in your network to connect", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient();

            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(textBoxClientIP.Text), int.Parse(textBoxClientPort.Text));
                client.Connect(ipEndPoint);
                MessageBox.Show("Connected to server\n\n" + textBoxClientIP.Text.ToString() + ":" + textBoxClientPort.Text.ToString(), "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxClientIP.Enabled = false;
                textBoxClientPort.Enabled = false;
                btnConnect.Enabled = false;
                comboBoxSrvIPSel.Enabled = false;
                lblSrvPort.Enabled = false;
                btnSrvStart.Enabled = false;

                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }

            catch
            {
                MessageBox.Show("Invalid IP address specified\n\nCould not connect to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSendMsg_Click(object sender, EventArgs e)
        {
            try
            {
                if (client.Connected)
                {
                    if (richTextBoxChatInput.Text != "")
                    {
                        textToSend = richTextBoxChatInput.Text;
                        backgroundWorker2.RunWorkerAsync();
                    }

                    richTextBoxChatInput.Text = "";
                }
                else
                {
                    // TO FIX:
                    // client.Connected still returns false after client reconnects to the server
                    MessageBox.Show("Lost connection to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Failed to send message\n\nNot connected to server/client or lost connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richTextBoxChatInput.Text = "";
            }
        }

        private void comboBoxLocalIPSel_DropDownClosed(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() => { comboBoxSrvIPSel.Select(0, 0); }));
        }

        private void richTextBoxChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (richTextBoxChatInput.Text != "")
                {
                    buttonSendMsg.PerformClick();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void textBoxChatScreen_TextChanged(object sender, EventArgs e)
        {
            // Set the current caret position to the end
            textBoxChatScreen.SelectionStart = textBoxChatScreen.Text.Length;
            // Scroll the chat automatically
            textBoxChatScreen.ScrollToCaret();
        }

        private void textBoxClientIPPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (textBoxClientIP.Text != "" && textBoxClientPort.Text != "")
                {
                    e.Handled = true;
                    btnConnect.PerformClick();
                }
            }
        }
    }
}