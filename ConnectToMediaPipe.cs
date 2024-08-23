using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Text;

public class ConnectToMediaPipe : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port = 12345;
    public bool startReceiving = true;
    public bool printToConsole = false;
    public string data;

    void Start()
    {
        InitializeReceiveData();
    }

    void InitializeReceiveData()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData))
        {
            IsBackground = true
        };
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        try
        {
            client = new UdpClient(port);
            while (startReceiving)
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataBytes = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataBytes);

                if (printToConsole)
                {
                    print(data);
                }
            }
        }
        catch (Exception err)
        {
            // To prevent the log from being overwhelmed when stopping receiving intentionally.
            if (startReceiving) print(err.ToString());
        }
    }

    // Ensure to close the UDP client and stop the thread when this object is destroyed.
    void OnDestroy()
    {
        StopReceivingData();
    }

    public void StopReceivingData()
    {
        startReceiving = false;
        if (client != null)
        {
            client.Close();
            client = null;
        }
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }
    }
}
