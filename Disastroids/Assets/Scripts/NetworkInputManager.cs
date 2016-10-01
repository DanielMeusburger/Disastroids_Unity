using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class NetworkInputManager : MonoBehaviour {

    UDPPacketIO packetIO = null;

    Thread ReadThread = null;
    bool ReaderRunning = false;

    public int ListeningPort, SendingPort;

    //DEBUG
    public static float X = 0f;

    void Start () {
        
        packetIO = GetComponent<UDPPacketIO>();
        packetIO.init("127.0.0.1", ListeningPort, SendingPort);

        ReadThread = new Thread(Read);
        ReaderRunning = true;
        ReadThread.IsBackground = true;
        ReadThread.Start();
    }

    void Destroy()
    {
        if(ReaderRunning)
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        if (ReaderRunning)
        {
            ReaderRunning = false;
            ReadThread.Abort();
        }
        if (packetIO != null && packetIO.IsOpen())
        {
            packetIO.Close();
            packetIO = null;
        }
    }

    private void Read()
    {
        try
        {
            while (ReaderRunning)
            {
                byte[] buffer = new byte[1000];
                int length = packetIO.ReceivePacket(buffer);
                //Debug.Log("received packed of len=" + length);
                if (length > 0)
                {
                    //DEBUG : If a UDP datagram is received, let's move the ship to the right indefinetly
                    Debug.Log("New Message");
                    Debug.Log(buffer);
                    X = 0.01f;
                }
                else
                    Thread.Sleep(20);
            }
        }
        catch (Exception e)
        {
            Debug.Log("ThreadAbortException"+e);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
