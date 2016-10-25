using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Text;

public delegate void NewControllerDelegate(string controllerIP);

public delegate void CommandReceivedDelegate(float x, float y, float z);

public class NetworkInputManager : MonoBehaviour {

    UDPPacketIO packetIO = null;

    Thread ReadThread = null;
    bool ReaderRunning = false;

    static NewControllerDelegate ncDelegate;

    public int ListeningPort;
    public int SendingPort;

    public static Dictionary<string, Controller> ConnectedControllers {
        get;
        protected set;
    }

    public static void setNewControllerDelegate(NewControllerDelegate ncDelegate)
    {
        NetworkInputManager.ncDelegate = ncDelegate;
    }

    void Start () {
        ConnectedControllers = new Dictionary<string,Controller>();

        packetIO = GetComponent<UDPPacketIO>();
        packetIO.init(SendingPort);

        //Start listening to UDP packets
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

    //Thread method waiting for UDP packets
    private void Read()
    {
        try
        {
            while (ReaderRunning)
            {
                byte[] buffer = new byte[1000];
                StringBuilder address = new StringBuilder();
                int length = packetIO.ReceivePacket(buffer, address);
                //Debug.Log("received packed of len=" + length);
                if (length > 0)
                {
                    string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer);
                    //Debug.Log(receivedMessage);
                    HandleMessage(receivedMessage, address.ToString());
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

    //Method called when a command is received from a controller
    private void HandleMessage(string udpMessage, string source)
    {
        //Here we check if controller sending the command is known or not. If not, we call the NewControllerDelegate
        if (!ConnectedControllers.ContainsKey(source))
        {
            if(ncDelegate!= null)
            {
                ncDelegate(source);
            }
        }
        //Parse the message from JSON
        UDPMessage message = JsonUtility.FromJson<UDPMessage>(udpMessage);
        
        try
        {
            ConnectedControllers[source].actions[message.type](message.x, message.y, message.z);
        } catch(Exception e)
        {
            Debug.Log(e);
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

//This class represents an UDP command
[Serializable]
public class UDPMessage
{
    public string type = "Yo";
    public float x, y, z = 0.5f ;
}


//This basic implementation of a controller is meant to be extended to handle more functionalities
public class Controller
{

    //Controller accelerometer
    public ControllerMovement Movement = new ControllerMovement();
    public ControllerMovement Rotation = new ControllerMovement();

    public Dictionary<string, CommandReceivedDelegate> actions = new Dictionary<string, CommandReceivedDelegate>();


    public Controller()
    {
        actions["Orientation"] = CommandOrientation;
        actions["Move"] = CommandMove;
    }

    public void CommandOrientation(float x, float y, float z)
    {
        Rotation.X = x;
        Rotation.Y = y;
        Rotation.Z = z;
    }

    public void CommandMove(float x, float y, float z)
    {
        Movement.X = x;
        Movement.Y = y;
        Movement.Z = z;
    }
}

public class ControllerMovement
{
    public float X, Y, Z = 0f;
}