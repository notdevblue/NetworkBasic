﻿using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Net;

namespace ConsoleApplication1
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        private static int userCount = 0;
        private static Mutex mut = new Mutex();


        static void Main(string[] args)
        {
            try
            {
                //IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
                //TcpListener serverSocket = new TcpListener(System.Net.IPAddress.Loopback, 8888);
                TcpClient clientSocket = default(TcpClient);
                int counter = 0;
                byte[] bytesFrom = new byte[1024];
                string dataFromClient = "";

                serverSocket.Start();
                Console.WriteLine("Chat Server Started ....");
                counter = 0;
                while ((true))
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();

                    dataFromClient = "";
                    /*
                    NetworkStream networkStream = clientSocket.GetStream();
                    int numBytesRead;

                    // 접속된 클라이언트의 닉네임 가져오기
                    while (networkStream.DataAvailable)
                    {
                        numBytesRead = networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        dataFromClient += Encoding.ASCII.GetString(bytesFrom, 0, numBytesRead);
                    }
                    int idx = dataFromClient.IndexOf("$");
                    if (idx >= 0)
                    {
                        dataFromClient = dataFromClient.Substring(0, idx);
                    }
                    */

                    // dataFromClient is nick name of the user.
                    //clientsList.Add(dataFromClient, clientSocket);
                    //broadcast(dataFromClient + " Joined ", dataFromClient, false);
                    //Console.WriteLine(dataFromClient + " Joined chat room ");
                    //clientsList.Add(userCount, clientSocket);

                    counter = userCount;
                    userCount++;

                    handleClient client = new handleClient();
                    clientsList.Add(counter, client);

                    client.startClient(clientSocket, clientsList, counter);
                }

                clientSocket.Close();
                serverSocket.Stop();
                Console.WriteLine("exit");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static TcpClient GetSocket(int id)
        {
            TcpClient socket = null;

            if (clientsList.ContainsKey(id))
            {
                handleClient hc = (handleClient)clientsList[id];
                socket = hc.clientSocket;
            }

            return socket;
        }

        public static void broadcast(string msg, string uName, bool flag)
        {
            mut.WaitOne();
            Byte[] broadcastBytes = null;

            if (flag == true)
            {
                broadcastBytes = Encoding.ASCII.GetBytes(uName + "$" + msg);
            }
            else
            {
                broadcastBytes = Encoding.ASCII.GetBytes(msg);
            }

            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                handleClient hc = (handleClient)Item.Value;
                broadcastSocket = hc.clientSocket;
                NetworkStream broadcastStream = broadcastSocket.GetStream();

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
            mut.ReleaseMutex();
        }  //end broadcast function

        public static void UserAdd(string clientNo, TcpClient clientSocket)
        {
            broadcast(clientNo + " Joined ", "", false);
            Console.WriteLine(clientNo + " Joined chat room ");
        }

        public static void UserLeft(int userID, string clientID)
        {
            if (clientsList.ContainsKey(userID))
            {
                broadcast(clientID + "$#Left#", clientID, false);
                Console.WriteLine("client Left:" + clientID);

                TcpClient clientSocket = GetSocket(userID);

                clientsList.Remove(userID);
                clientSocket.Close();
            }
        }

        ~Program()
        {
            mut.Dispose();
        }
    }//end Main class


    public class handleClient
    {
        const string COMMAND_MOVE = "#Move#";
        const string COMMAND_ENTER = "#Enter#";
        const string COMMAND_HISTORY = "#History#";

        public TcpClient clientSocket;
        public int userID;
        public string clientID;
        public float targetPosX;
        public float targetPosY;

        private Hashtable clientsList;
        private bool noConnection = false;

        public void startClient(TcpClient inClientSocket, Hashtable cList, int userSerial)
        {
            userID = userSerial;
            this.clientSocket = inClientSocket;
            // this.clNo = clineNo;
            this.clientsList = cList;

            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        // 기존 사용자 정보의 전달
        private void SendHistory(NetworkStream dataStream)
        {
            string history = "$" + COMMAND_HISTORY;
            bool first = true;
            foreach (DictionaryEntry Item in clientsList)
            {
                handleClient hc = (handleClient)Item.Value;
                if (!first)
                {
                    history += ",";
                }
                else
                {
                    first = false;
                }

                history += hc.clientID + "," + hc.targetPosX.ToString() + "," + hc.targetPosY.ToString();
            }

            Console.WriteLine("final history = " + history);

            Byte[] dataBytes = null;
            dataBytes = Encoding.ASCII.GetBytes(history);

            dataStream.Write(dataBytes, 0, dataBytes.Length);
            dataStream.Flush();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[1024];
            string dataFromClient = "";
            //Byte[] sendBytes = null;
            //string serverResponse = null;
            string rCount = null;
            requestCount = 0;
            NetworkStream networkStream = clientSocket.GetStream();

            while (!noConnection)
            {
                try
                {
                    requestCount = requestCount + 1;

                    // networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    int numBytesRead;
                    if (!SocketConnected(clientSocket.Client))
                    {
                        // socket closed
                        noConnection = true;
                    }
                    else
                    {
                        if (networkStream.DataAvailable)
                        {
                            dataFromClient = "";
                            while (networkStream.DataAvailable)
                            {
                                numBytesRead = networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                                dataFromClient += Encoding.ASCII.GetString(bytesFrom, 0, numBytesRead);
                            }

                            // dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                            int idx = dataFromClient.IndexOf("$");

                            // check if ID has been sent
                            if (clientID == null && idx > 0)
                            {
                                // get the ID part only
                                clientID = dataFromClient.Substring(0, idx);
                                Console.WriteLine(clientID + " enters chat room.");
                                Program.broadcast(clientID + "$" + COMMAND_ENTER, clientID, false);
                                SendHistory(networkStream);
                            }

                            // get the message part only
                            int pos = idx + 1;
                            if (pos < dataFromClient.Length)
                            {
                                dataFromClient = dataFromClient.Substring(pos, dataFromClient.Length - pos);
                                Console.WriteLine("From client - " + clientID + " : " + dataFromClient);

                                // 비지니스 로직의 처리
                                if (dataFromClient.StartsWith(COMMAND_MOVE))
                                {
                                    string remain = dataFromClient.Substring(COMMAND_MOVE.Length);
                                    var strs = remain.Split(',');
                                    try
                                    {
                                        targetPosX = float.Parse(strs[0]);
                                        targetPosY = float.Parse(strs[1]);
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                                Program.broadcast(dataFromClient, clientID, true);
                            }
                            else
                            {
                                dataFromClient = "";
                            }
                            rCount = Convert.ToString(requestCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // connection broken - delete current user.
                    noConnection = true;
                    Console.WriteLine("Error:" + ex.ToString());
                }
            }//end while

            Program.UserLeft(userID, clientID);
            Program.broadcast("User left:" + clientID, "", false);

        }//end doChat
    } //end class handleClinet
}//end namespace