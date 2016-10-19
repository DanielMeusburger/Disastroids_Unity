using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.Text;

public delegate void NewControllerDelegate(string controllerIP);

public class NetworkInputManager : MonoBehaviour {

    UDPPacketIO packetIO = null;

    Thread ReadThread = null;
    bool ReaderRunning = false;

    static NewControllerDelegate ncDelegate;

    public int ListeningPort;
    public int SendingPort;

    public static System.Collections.Generic.Dictionary<string, Controller> ConnectedControllers {
        get;
        protected set;
    }

    public static void setNewControllerDelegate(NewControllerDelegate ncDelegate)
    {
        NetworkInputManager.ncDelegate = ncDelegate;
    }

    void Start () {

        ConnectedControllers = new System.Collections.Generic.Dictionary<string,Controller>();

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

    private void HandleMessage(string udpMessage, string source)
    {
        if(!ConnectedControllers.ContainsKey(source))
        {
            ConnectedControllers[source] = new Controller();
            if(ncDelegate!= null)
            {
                ncDelegate(source);
            }
        }
        UDPMessage message = JsonUtility.FromJson<UDPMessage>(udpMessage);
        switch(message.type)
        {
            case "Move":
                ConnectedControllers[source].Movement.X = message.x;
                ConnectedControllers[source].Movement.Y = message.y;
                ConnectedControllers[source].Movement.Z = message.z;
                break;
            case "Fire":
                ConnectedControllers[source].FireCommandRegistered = true;
                break;
            case "Orientation":
                ConnectedControllers[source].Rotation.X = message.x;
                ConnectedControllers[source].Rotation.Y = message.y;
                ConnectedControllers[source].Rotation.Z = message.z;
                break;
            case "Charge":
                ConnectedControllers[source].ChargeCommandRegistered = true;
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

public class Controller
{

    //Controller accelerometer
    public ControllerMovement Movement = new ControllerMovement();
    public ControllerMovement Rotation = new ControllerMovement();

    public bool FireCommandRegistered = false;

    private bool _chargeCommnandRegistered = false;
    public bool ChargeCommandRegistered {
        get
        {
            bool returnvalue = _chargeCommnandRegistered;
            _chargeCommnandRegistered = false;
            return returnvalue;
            //return _chargeCommandRegistered;
        }
        set
        {
            _chargeCommnandRegistered = value;
        }
    }
}

public class ControllerMovement
{
    public float X, Y, Z = 0f;
}