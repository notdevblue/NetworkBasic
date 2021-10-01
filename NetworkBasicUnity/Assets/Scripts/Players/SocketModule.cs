using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketModule : MonoBehaviour
{
    static SocketModule instance = null;

    private TcpClient clientSocket;
    private GameManager gm;

    private NetworkStream serverStream = default(NetworkStream);
    private Queue<string> msgQueue;
    private string nickName;

    bool bRunning = false;

    public string ipaddr = "172.31.0.235";

    public static SocketModule GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        msgQueue = new Queue<string>();
        gm = GetComponent<GameManager>();
    }

    public void Login(string id)
    {
        try
        {
            if (!bRunning)
            {
                clientSocket = new TcpClient();
                clientSocket.Connect(ipaddr, 8888);
                serverStream = clientSocket.GetStream();

                byte[] outStream = Encoding.ASCII.GetBytes(id + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
                bRunning = true;
                nickName = id;
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("Login error");
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }

    public void SendData(string str)
    {
        if(bRunning && serverStream != null) // <= Safety code
        {
            byte[] outStream = Encoding.ASCII.GetBytes("$" + str);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
    }

    private void StopThread()
    {
        bRunning = false;
    }

    public void Logout()
    {
        if(bRunning)
        {
            StopThread();
            msgQueue.Clear();
            nickName = "";
        }
        
        if(serverStream != null)
        {
            serverStream.Close();
            serverStream = null;
        }

        if(clientSocket != null)
        {
            clientSocket.Close();
        }
    }

    public bool IsOnline()
    {
        return bRunning;
    }

    public string GetNextData()
    {
        if(msgQueue.Count > 0)
        {
            string nextMsg = msgQueue.Dequeue();
            return nextMsg;
        }
        return null;
    }

    private void getMessage() // 스레드 처리
    {
        byte[] inStream = new byte[1024];
        string returndata = "";

        try
        {
            while(bRunning)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                buffSize = clientSocket.ReceiveBufferSize;
                int numBytesRead;

                if(serverStream.DataAvailable)
                {
                    returndata = "";
                    while(serverStream.DataAvailable)
                    {
                        numBytesRead = serverStream.Read(inStream, 9, inStream.Length);
                        returndata += Encoding.ASCII.GetString(inStream, 0, numBytesRead);
                    }

                    //gm.QueueCommand(returnadata);
                    Debug.Log(returndata);
                }

            }
        }
        catch(Exception ex)
        {
            StopThread();
        }
    }
}
