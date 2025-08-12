using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ChatClientServer
{
    internal class Server
    {
        private static Form1 form = Form1.Self;

        private static Thread srvThread;
        private static TcpListener listener;
        private static readonly List<TcpClient> connectedClients = new List<TcpClient>();
        private static readonly object clientLock = new object();

        public static bool isSrvRunning = false;

        public static void StartSrv()
        {
            if (form.textBoxSrvPort.Text != "" && int.TryParse(form.textBoxSrvPort.Text, out int port) && port < 65536)
            {
                listener = new TcpListener(IPAddress.Any, port);
                try
                {
                    listener.Start();
                    isSrvRunning = true;

                    srvThread = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        while (isSrvRunning)
                        {
                            try
                            {
                                TcpClient newClient = listener.AcceptTcpClient();
                                if (isSrvRunning)
                                {
                                    lock (clientLock)
                                    {
                                        connectedClients.Add(newClient);
                                    }
                                    Thread clientThread = new Thread(() => HandleClient(newClient));
                                    clientThread.IsBackground = true;
                                    clientThread.Start();
                                }
                                else
                                {
                                    newClient.Close();
                                    break;
                                }
                            }
                            catch (ObjectDisposedException)
                            {
                                break;
                            }
                            catch (SocketException)
                            {
                                if (isSrvRunning)
                                {
                                    MessageBox.Show("Server socket error", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                break;
                            }
                        }
                    });
                    srvThread.Start();

                    form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 20);
                    form.lblStatus.ForeColor = Color.Green;
                    form.lblStatus.Text = "Server running on port:\n" + port;
                    form.EnableDisableControls(true, true);
                    MessageBox.Show("Server started and running on port: " + port,
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    form.Text = $"Chat Client/Server ({port})";

                    IO.AddOrUpdateAppSettings("SrvPort", port.ToString());

                    // Connect the server to itself as a client so it can also chat
                    string localIP = "127.0.0.1";
                    int localPort = int.Parse(form.textBoxSrvPort.Text);
                    form.textBoxClientIP.Text = localIP;
                    form.textBoxClientPort.Text = localPort.ToString();
                    Client.ConnectToSrv();
                }
                catch (SocketException)
                {
                    MessageBox.Show("Port already in use!\n\nTry again with another port", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (form.textBoxSrvPort.Text == "")
                {
                    MessageBox.Show("Server port cannot be empty!\n\n" +
                        "Attempting to set a random unused port...", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    try
                    {
                        form.textBoxSrvPort.Text = Server.GetRandomUnusedPort().ToString();
                        StartSrv();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to find a free port!\n\nServer cannot be started.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Port out of range!\n\nMake sure it is less than 65536", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void StopSrv()
        {
            try
            {
                isSrvRunning = false;

                if (form.backgroundWorker1.IsBusy)
                {
                    form.backgroundWorker1.CancelAsync();
                }
                if (form.backgroundWorker2.IsBusy)
                {
                    form.backgroundWorker2.CancelAsync();
                }

                if (Client.client != null && Client.client.Connected)
                {
                    try
                    {
                        Program.STW?.Close();
                        Program.STR?.Close();
                        Client.client.Close();
                        Client.client = null;
                    }
                    catch { }
                }

                lock (clientLock)
                {
                    foreach (var client in connectedClients.ToList())
                    {
                        try
                        {
                            client.Close();
                        }
                        catch { }
                    }
                    connectedClients.Clear();
                }

                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }

                Thread.Sleep(100);

                form.textBoxClientIP.Text = "";
                form.textBoxClientPort.Text = "";
                form.lblStatus.Font = new Font(form.lblStatus.Font.Name, 28);
                form.lblStatus.ForeColor = Color.Black;
                form.lblStatus.Text = "OR";
                form.EnableDisableControls(false, true);
                form.Text = "Chat Client/Server";

                MessageBox.Show("Server stopped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping server: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
            {
                string message;
                try
                {
                    while ((message = reader.ReadLine()) != null)
                    {
                        BroadcastMessage(message, client);
                    }
                }
                catch { }
                finally
                {
                    lock (clientLock)
                    {
                        connectedClients.Remove(client);
                    }

                    client.Close();
                }
            }
        }

        private static void BroadcastMessage(string message, TcpClient sender)
        {
            lock (clientLock)
            {
                foreach (var client in connectedClients)
                {
                    if (client != sender && client.Connected)
                    {
                        try
                        {
                            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                            writer.WriteLine(message);
                        }
                        catch { }
                    }
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