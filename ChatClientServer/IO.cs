using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ChatClientServer
{
    internal class IO
    {
        private static Form1 form = Form1.Self;

        private static FormPassword formPassword = new FormPassword();

        private static string localDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " +
            CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

        public static void ChatHistoryFileCheck()
        {
            if (!File.Exists(Program.chatHistory))
            {
                using (FileStream fs = File.Create(Program.chatHistory)) { };

                DialogResult result = MessageBox.Show("Would you like to encrypt the chat history file with a password?", "Welcome",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    FormPassword formPassword = new FormPassword();
                    formPassword.ShowDialog();

                    if (formPassword.IsBtnOKPressed && formPassword.textBoxPassword.Text != "")
                    {
                        using (StreamWriter sw = File.AppendText(Program.chatHistory))
                        {
                            sw.Write(Crypto.Encrypt("control_line_DO_NOT_DELETE\n\n", formPassword.textBoxPassword.Text));
                        }

                        MessageBox.Show("The history file will now be encrypted and decrypted using that password.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(Program.chatHistory))
                        {
                            sw.Write("control_line_DO_NOT_DELETE\n\n");
                        }

                        MessageBox.Show("No password was entered!\n\nYour chat history file will be unencrypted! " +
                            "If this was a mistake, delete the file, restart the app and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(Program.chatHistory))
                    {
                        sw.Write("control_line_DO_NOT_DELETE\n\n");
                    }

                    MessageBox.Show("The chat history file will not be encrypted!\n\n" +
                        "If this was a mistake, delete the file, restart the app and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (new FileInfo(Program.chatHistory).Length != 0)
                {
                    int maxTries = 3;
                    Program.chatHistoryLines = File.ReadAllLines(Program.chatHistory);

                    if (File.ReadLines(Program.chatHistory).First() != "control_line_DO_NOT_DELETE")
                    {
                        formPassword.lblPassword.Font = new Font(formPassword.lblPassword.Font.FontFamily, 9);
                        formPassword.lblPassword.Left = 55;
                        formPassword.lblPassword.Text = "Enter your password";

                        while (maxTries <= 3)
                        {
                            formPassword.ShowDialog();

                            try
                            {
                                if (Crypto.Decrypt(File.ReadLines(Program.chatHistory).First(), formPassword.textBoxPassword.Text.ToString()) != "control_line_DO_NOT_DELETE")
                                {
                                    foreach (var line in Program.chatHistoryLines)
                                    {
                                        Program.decryptedText = Crypto.Decrypt(line.ToString(), formPassword.textBoxPassword.Text);

                                        form.textBoxChatScreen.Text += Program.decryptedText;
                                    }

                                    string[] linesFinal = form.textBoxChatScreen.Text.Split(Environment.NewLine.ToCharArray()).Skip(2).ToArray();

                                    form.textBoxChatScreen.Lines = linesFinal.ToArray();
                                    form.textBoxChatScreen.SelectionStart = form.textBoxChatScreen.Text.Length;
                                    form.textBoxChatScreen.ScrollToCaret();

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

                                    form.notifyIcon1.Visible = false;
                                    Application.Exit();
                                }
                            }
                        }
                    }
                    else
                    {
                        var linesFinal = File.ReadAllLines(Program.chatHistory).Skip(2);

                        form.textBoxChatScreen.Lines = linesFinal.ToArray();
                        form.textBoxChatScreen.SelectionStart = form.textBoxChatScreen.Text.Length;
                        form.textBoxChatScreen.ScrollToCaret();
                    }
                }
            }
        }

        public static void OpenHistoryFile()
        {
            if (File.Exists(Program.chatHistory))
            {
                Process.Start(Program.chatHistory);
            }
            else
            {
                MessageBox.Show("The chat history file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void OpenHistoryFileLocation()
        {
            Process.Start("explorer.exe", $"/select, {Program.chatHistory}");
        }

        public static void ClearHistoryFile()
        {
            DialogResult result = MessageBox.Show("You are about to clear the chat history file!\n\nAre you sure you want to proceed?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (new FileInfo(Program.chatHistory).Length == 0)
                {
                    MessageBox.Show("Nothing to clear! File is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        using (StreamWriter sw = File.CreateText(Program.chatHistory))
                        {
                            sw.Write(Crypto.Encrypt("control_line_DO_NOT_DELETE\n\n", formPassword.textBoxPassword.Text));
                        }
                    }
                    catch
                    {
                        using (StreamWriter sw = File.CreateText(Program.chatHistory))
                        {
                            sw.Write("control_line_DO_NOT_DELETE\n\n");
                        }
                    }

                    MessageBox.Show("Chat history file cleared.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static void DeleteHistoryFileAndRestartApp()
        {
            DialogResult result = MessageBox.Show("You are about to delete the chat history file and restart the app!\n\nAre you sure you want to proceed?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                File.Delete(Program.chatHistory);
                Application.Restart();
            }
        }

        public static void ReceiveFromClient()
        {
            while (Client.client.Connected)
            {
                try
                {
                    Program.receive = Program.STR.ReadLine();
                    string receiveFormatted = $"[{DateTime.Now.ToString(localDateTimeFormat)}]\nPerson B: {Program.receive}\n\n";

                    form.textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                    {
                        form.textBoxChatScreen.AppendText(receiveFormatted);

                        if (File.ReadLines(Program.chatHistory).First() != "control_line_DO_NOT_DELETE")
                        {
                            string encryptedBuffer = File.ReadAllText(Program.chatHistory);

                            try
                            {
                                Program.decryptedBuffer = Crypto.Decrypt(encryptedBuffer, formPassword.textBoxPassword.Text) + receiveFormatted;
                            }
                            catch
                            {
                                Program.decryptedBuffer = encryptedBuffer;
                            }

                            Program.encryptedText = Crypto.Encrypt(Program.decryptedBuffer, formPassword.textBoxPassword.Text);

                            using (StreamWriter sw = File.CreateText(Program.chatHistory))
                            {
                                sw.Write(Program.encryptedText);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = File.AppendText(Program.chatHistory))
                            {
                                sw.Write(receiveFormatted);
                            }
                        }

                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.FlashNotification();
                        }
                    }));

                    Program.receive = "";
                }

                catch
                {
                    MessageBox.Show("Lost connection to remote endpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                }
            }
        }

        public static void ReceiveFromServer()
        {
            if (Client.client.Connected)
            {
                Program.STW.WriteLine(Program.textToSend);
                string textToSendFormatted = $"[{DateTime.Now.ToString(localDateTimeFormat)}]\nPerson A: {Program.textToSend}\n\n";

                form.textBoxChatScreen.Invoke(new MethodInvoker(delegate ()
                {
                    form.textBoxChatScreen.AppendText(textToSendFormatted);
                }
                ));

                if (File.ReadLines(Program.chatHistory).First() != "control_line_DO_NOT_DELETE")
                {
                    string encryptedBuffer = File.ReadAllText(Program.chatHistory);
                    string decryptedBuffer = Crypto.Decrypt(encryptedBuffer, formPassword.textBoxPassword.Text) + textToSendFormatted;
                    Program.encryptedText = Crypto.Encrypt(decryptedBuffer, formPassword.textBoxPassword.Text);

                    using (StreamWriter sw = File.CreateText(Program.chatHistory))
                    {
                        sw.Write(Program.encryptedText);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(Program.chatHistory))
                    {
                        sw.Write(textToSendFormatted);
                    }
                }
            }
            else
            {
                MessageBox.Show("Sending Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            form.backgroundWorker2.CancelAsync();
        }

        public static void SendMsg()
        {
            try
            {
                if (Client.client.Connected && form.richTextBoxChatInput.Text != "")
                {
                    Program.textToSend = form.richTextBoxChatInput.Text;
                    form.backgroundWorker2.RunWorkerAsync();
                    form.richTextBoxChatInput.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Failed to send message. Reason:\n\nNo connection to remote endpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.richTextBoxChatInput.Text = "";
            }
        }
    }
}