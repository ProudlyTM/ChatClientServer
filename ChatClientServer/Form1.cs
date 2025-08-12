using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ChatClientServer
{
    public partial class Form1 : Form
    {
        public static Form1 Self;

        public bool IsSrv;

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

                if (ConfigurationManager.AppSettings["SysTrayNotification"] == "enabled")
                {
                    notifyIcon1.ShowBalloonTip(1000);
                    IO.AddOrUpdateAppSettings("SysTrayNotification", "disabled");
                }
            }
        }

        public void EnableDisableControls(bool AreControlsEnabled, bool IsSrv)
        {
            if (AreControlsEnabled)
            {
                this.IsSrv = IsSrv;

                textBoxClientIP.Enabled = false;
                textBoxClientPort.Enabled = false;
                textBoxSrvPort.Enabled = false;
                btnConnect.Enabled = false;
                btnSrvStart.Enabled = false;
                btnGenRandomPort.Enabled = false;
                comboBoxSrvIPSel.Enabled = false;

                if (IsSrv)
                {
                    btnSrvStop.Enabled = true;
                    btnDisconnect.Enabled = false;
                }
                else
                {
                    btnDisconnect.Enabled = true;
                    btnSrvStop.Enabled = false;
                }
            }
            else
            {
                if (!IsSrv)
                {
                    this.IsSrv = false;
                }

                textBoxClientIP.Enabled = true;
                textBoxClientPort.Enabled = true;
                textBoxSrvPort.Enabled = true;
                btnConnect.Enabled = true;
                btnSrvStart.Enabled = true;
                btnGenRandomPort.Enabled = true;
                btnSrvStop.Enabled = false;
                btnDisconnect.Enabled = false;
                comboBoxSrvIPSel.Enabled = true;

                lblStatus.Font = new Font(lblStatus.Font.Name, 28);
                lblStatus.ForeColor = Color.Black;
                lblStatus.Text = "OR";
            }
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        public Form1()
        {
            InitializeComponent();
            Self = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = Properties.Settings.Default.F1State;
            Location = Properties.Settings.Default.F1Location;
            Size = Properties.Settings.Default.F1Size;
            Server.GetLocalIPs();
            ContextMenu contextMenu = new ContextMenu();
            comboBoxSrvIPSel.SelectedIndex = 0;
            notifyIcon1.ContextMenu = contextMenu;
            contextMenu.MenuItems.Add("Exit", ExitApplication);

            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerSupportsCancellation = true;

            IO.ChatHistoryFileCheck();
            textBoxSrvPort.Text = ConfigurationManager.AppSettings["SrvPort"];
            IO.GenOrLoadNick();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                while (!worker.CancellationPending && Client.client?.Connected == true)
                {
                    IO.ReceiveFromClient();

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    if (Client.client?.Connected != true)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(10);
                }
            }
            catch (Exception)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (!worker.CancellationPending && Client.client?.Connected == true)
            {
                try
                {
                    IO.ReceiveFromServer();
                }
                catch (Exception)
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        if (!IsSrv)
                        {
                            MessageBox.Show("Failed to send message. Connection lost.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        lblStatus.Font = new Font(lblStatus.Font.Name, 28);
                        lblStatus.ForeColor = Color.Black;
                        lblStatus.Text = "OR";
                        EnableDisableControls(false, false);
                        Text = "Chat Client/Server";

                        try
                        {
                            Program.STW?.Close();
                            Program.STR?.Close();
                            Client.client?.Close();
                            Client.client = null;
                        }
                        catch { }
                    }));
                }
            }

            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
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
            IO.OpenHistoryFile();
        }

        private void openHistoryFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IO.OpenHistoryFileLocation();
        }

        private void clearHistoryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IO.ClearHistoryFile();
        }

        private void deleteHistoryFileAndRestartAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IO.DeleteHistoryFileAndRestartApp();
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
            Server.StartSrv();
        }

        private void btnSrvStop_Click(object sender, EventArgs e)
        {
            Server.StopSrv();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Client.ConnectToSrv();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Client.DisconnectFromSrv();
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            IO.SendMsg();
        }

        private void textBoxSrvPort_TextChanged(object sender, EventArgs e)
        {
            Program.srvPort = textBoxSrvPort.Text;
        }

        private void btnGenRandomPort_Click(object sender, EventArgs e)
        {
            textBoxSrvPort.Text = Server.GetRandomUnusedPort().ToString();
        }

        private void btnGenRandomPort_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Click to auto-fill a random unused port", btnGenRandomPort);
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
                    btnSendMsg.PerformClick();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void textBoxChatScreen_TextChanged(object sender, EventArgs e)
        {
            //Scroll the text to the bottom on new message sent/received
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

        private void textBoxSrvPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (textBoxSrvPort.Text != "")
                {
                    Program.srvPort = textBoxSrvPort.Text;
                    e.Handled = true;
                    btnSrvStart.PerformClick();
                }
            }
        }

        private void btnSetNick_Click(object sender, EventArgs e)
        {
            IO.SetNick();
        }

        private void textBoxNick_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnSetNick.PerformClick();
            }
        }
    }
}