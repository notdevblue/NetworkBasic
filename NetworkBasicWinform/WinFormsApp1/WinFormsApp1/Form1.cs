using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new();
        NetworkStream serverStream = default;
        string readData = null;
        bool stopRunning = false;

        public string addr = "localhost";
        public ushort port = 32000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //send
        {
            byte[] outStream = Encoding.ASCII.GetBytes(InputText.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void ConnetServer_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected) return;
            if (InputNickname.Text.Trim() == "") return;

            readData = "Connected to Chat Server...";
            msg();
            
            clientSocket.Connect(addr, port);
            serverStream = clientSocket.GetStream();

            byte[] outStream = Encoding.ASCII.GetBytes(InputNickname.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new(getMessage);
            ctThread.Start();
        }

        private void getMessage()
        {
            byte[] inStream = new byte[1024];
            string returnData = "";

            try
            {
                while (!stopRunning)
                {
                    serverStream = clientSocket.GetStream();
                    int bufferSize = 0;
                    bufferSize = clientSocket.ReceiveBufferSize;
                    int numBytesRead;

                    if (serverStream.DataAvailable)
                    {
                        returnData = "";
                        while (serverStream.DataAvailable)
                        {
                            numBytesRead = serverStream.Read(inStream, 0, inStream.Length);
                            returnData += Encoding.ASCII.GetString(inStream, 0, numBytesRead);
                        }

                        readData = "" + returnData;
                        msg();
                    }
                }
            }
            catch (Exception e)
            {
                stopRunning = true;
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                textHistory.Text = string.Concat(Environment.NewLine + " >> " + readData, textHistory.Text);
            }
        }

        private void textHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputNickname_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopRunning = true;
            serverStream?.Close();
            clientSocket?.Close();
        }
    }
}
