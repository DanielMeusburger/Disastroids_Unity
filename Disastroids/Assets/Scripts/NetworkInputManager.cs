using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class NetworkInputManager : MonoBehaviour {

    UDPPacketIO packetIO = null;

    Thread ReadThread = null;
    bool ReaderRunning = false;

    public int ListeningPort;
    public int SendingPort;

    //DEBUG
    //Controller accelerometer
    public static ControllerMovement Movement = new ControllerMovement();
    public static ControllerMovement Rotation = new ControllerMovement();

    public static bool FireCommandRegistered = false;

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
                    string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer);
                    //DEBUG : If a UDP datagram is received, let's move the ship to the right indefinetly
                    Debug.Log("New Message");
                    Debug.Log(receivedMessage);
                    //X = 0.01f;

                    HandleMessage(receivedMessage);
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

    private void HandleMessage(string udpMessage)
    {
        UDPMessage message = JsonUtility.FromJson<UDPMessage>(udpMessage);
        switch(message.type)
        {
            case "Move":
                Movement.X = message.x;
                Movement.Y = message.y;
                Movement.Z = message.z;
                break;
            case "Fire":
                FireCommandRegistered = true;
                break;
            case "Rotate":
                Rotation.X = message.x;
                Rotation.Y = message.y;
                Rotation.Z = message.z;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

[Serializable]
public class UDPMessage
{
    public string type = "Yo";
    public float x, y, z = 0.5f ;
}

public class ControllerMovement
{
    public float X, Y, Z = 0f;
}