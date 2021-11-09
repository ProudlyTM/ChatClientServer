using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ChatClientServer
{
    public partial class Form1 : Form
    {
        private string srvPort, receive, textToSend, encryptedText, decryptedText;

        private static readonly string chatHistory = Directory.GetCurrentDirectory() + "\\history.log";

        private static string[] chatHistoryLines;

        private readonly FormPassword child = new FormPassword();

        private StreamReader STR;
        private StreamWriter STW;
        private TcpClient client;
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall)
            {
                Properties.Settings.Default.F1State = WindowState;

                if (WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.F1Location = Location;
                    Properties.Settings.Default.F1Size = Size;
                }
                else
                {
                    Properties.Settings.Default.F1Location = RestoreBounds.Location;
                    Properties.Settings.Default.F1Size = RestoreBounds.Size;
                }

                Properties.Settings.Default.Save();

                return;
            }
            else
            {
                e.Cancel = true;
                Hide();
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void GetLocalIPs()
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

        private void ExitApplication(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private static int GetRandomUnusedPort()
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
            WindowState = Properties.Settings.Default.F1State;
            Location = Properties.Settings.Default.F1Location;
            Size = Properties.Settings.Default.F1Size;

            GetLocalIPs();
            ContextMenu contextMenu = new ContextMenu();
            comboBoxSrvIPSel.SelectedIndex = 0;
            srvPort = GetRandomUnusedPort().ToString();
            lblSrvPort.Text += srvPort;
            notifyIcon1.ContextMenu = contextMenu;
            contextMenu.MenuItems.Add("Exit", ExitApplication);

            if (!File.Exists(chatHistory))
            {
                using (FileStream fs = File.Create(chatHistory)) { };

                DialogResult result = MessageBox.Show("Would you like to encrypt the chat history file with a password?", "Welcome",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    FormPassword child = new FormPassword();
                    child.ShowDialog();

                    if (child.IsBtnOKPressed && child.textBoxPassword.Text != "")
                    {
                        using (StreamWriter sw = File.AppendText(chatHistory))
                        {
                            sw.Write(StringCipher.Encrypt("control_line_DO_NOT_DELETE\n\n", child.textBoxPassword.Text));
                        }

                        MessageBox.Show("The history file will now be encrypted and decrypted using that password.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(chatHistory))
                        {
                            sw.Write("control_line_DO_NOT_DELETE\n\n");
                        }

                        MessageBox.Show("No password was entered!\n\nYour chat history file will be unencrypted! " +
                            "If this was a mistake, delete the file, restart the app and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(chatHistory))
                    {
                        sw.Write("control_line_DO_NOT_DELETE\n\n");
                    }

                    MessageBox.Show("The chat history file will not be encrypted!\n\n" +
                        "If this was a mistake, delete the file, restart the app and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (new FileInfo(chatHistory).Length != 0)
                {
                    int maxTries = 3;
                    chatHistoryLines = File.ReadAllLines(chatHistory);

                    if (File.ReadLines(chatHistory).First() != "control_line_DO_NOT_DELETE")
                    {
                        child.lblPassword.Text = "Enter your password";

                        while (maxTries <= 3)
                        {
                            child.ShowDialog();

                            try
                            {
                                if (StringCipher.Decrypt(File.ReadLines(chatHistory).First(), child.textBoxPassword.Text.ToString()) != "control_line_DO_NOT_DELETE")
                                {
                                    foreach (var line in chatHistoryLines)
                                    {
                                        decryptedText = StringCipher.Decrypt(line.ToString(), child.textBoxPassword.Text);

                                        textBoxChatScreen.Text += decryptedText;
                                    }

                                    string[] linesFinal = textBoxChatScreen.Text.Split(Environment.NewLine.ToCharArray()).Skip(2).ToArray();

                                    textBoxChatScreen.Lines = linesFinal.ToArray();
                                    textBoxChatScreen.SelectionStart = textBoxChatScreen.Text.Length;
                                    textBoxChatScreen.ScrollToCaret();

                                    MessageBox.Show("Welcome!\n\nThe chat history file has been successfully decrypted!", "Welcome",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    break;
                                }
                            }
                            catch
                            {
                                maxTries--;

                                if (maxTries == 0)
                                {
                                    MessageBox.Show("Tried to decrypt and load the chat history file, but failed!\n\nEnsure you are entering the correct password.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    notifyIcon1.Visible = false;
                                    Application.Exit();
                                }
                            }
                        }
                    }
                    else
                    {
                        var linesFinal = File.ReadAllLines(chatHistory).Skip(2);

                        textBoxChatScreen.Lines = linesFinal.ToArray();
                        textBoxChatScreen.SelectionStart = textBoxChatScreen.Text.Length;
                        textBoxChatScreen.ScrollToCaret();
                    }
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    string receiveFormatted = $"[{DateTime.Now:dd/MM/yyyy HH:mm}]\nPerson B: {receive}\n\n";

                    textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                    {
                        textBoxChatScreen.AppendText(receiveFormatted);

                        if (File.ReadLines(chatHistory).First() != "control_line_DO_NOT_DELETE")
                        {
                            string encryptedBuffer = File.ReadAllText(chatHistory);
                            string decryptedBuffer = StringCipher.Decrypt(encryptedBuffer, child.textBoxPassword.Text) + receiveFormatted;
                            encryptedText = StringCipher.Encrypt(decryptedBuffer, child.textBoxPassword.Text);

                            using (StreamWriter sw = File.CreateText(chatHistory))
                            {
                                sw.Write(encryptedText);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = File.AppendText(chatHistory))
                            {
                                sw.Write(receiveFormatted);
                            }
                        }
                        
                        if (WindowState == FormWindowState.Minimized)
                        {
                            this.FlashNotification();
                        }
                    }));

                    receive = "";
                }

                catch
                {
                    MessageBox.Show("Lost connection to remote endpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(textToSend);
                string textToSendFormatted = $"[{DateTime.Now:dd/MM/yyyy HH:mm}]\nPerson A: {textToSend}\n\n";

                textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                {
                    textBoxChatScreen.AppendText(textToSendFormatted);
                }
                ));

                if (File.ReadLines(chatHistory).First() != "control_line_DO_NOT_DELETE")
                {
                    string encryptedBuffer = File.ReadAllText(chatHistory);
                    string decryptedBuffer = StringCipher.Decrypt(encryptedBuffer, child.textBoxPassword.Text) + textToSendFormatted;
                    encryptedText = StringCipher.Encrypt(decryptedBuffer, child.textBoxPassword.Text);

                    using (StreamWriter sw = File.CreateText(chatHistory))
                    {
                        sw.Write(encryptedText);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(chatHistory))
                    {
                        sw.Write(textToSendFormatted);
                    }
                }
            }
            else
            {
                MessageBox.Show("Sending Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            backgroundWorker2.CancelAsync();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void clearCurrentHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxChatScreen.Text = "";
            MessageBox.Show("Current history cleared.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openHistoryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(chatHistory))
            {
                Process.Start(chatHistory);
            }
            else
            {
                MessageBox.Show("The chat history file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openHistoryFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", $"/select, {chatHistory}");
        }

        private void clearHistoryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You are about to clear the chat history file!\n\nAre you sure you want to proceed?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (new FileInfo(chatHistory).Length == 0)
                {
                    MessageBox.Show("Nothing to clear! File is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // TODO: Check if the chat history file was encrypted and if yes, re-add and re-encrypt the control lines using the same password
                    using (StreamWriter sw = File.CreateText(chatHistory))
                    {
                        sw.Write("control_line_DO_NOT_DELETE\n\n");
                    }

                    MessageBox.Show("Chat history file cleared.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text != "OR")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Really exit?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    notifyIcon1.Visible = false;
                    Application.Exit();
                }
            }
            else
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            }
        }

        private void sourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ProudlyTM/ChatClientServer");
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

            lblStatus.Font = new Font(lblStatus.Font.Name, 20);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Text = "Server running on port:\n" + srvPort;

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

                lblStatus.Font = new Font(lblStatus.Font.Name, 20);
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Connected to\n" + textBoxClientIP.Text + ":" + textBoxClientPort.Text;
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
                if (client.Connected && richTextBoxChatInput.Text != "")
                {
                    textToSend = richTextBoxChatInput.Text;
                    backgroundWorker2.RunWorkerAsync();
                    richTextBoxChatInput.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Failed to send message. Reason:\n\nNo connection to remote endpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Scroll the text to the bottom on new message sent/received
            textBoxChatScreen.SelectionStart = textBoxChatScreen.Text.Length;
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