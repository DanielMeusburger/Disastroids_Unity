using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;


// UdpPacket provides packet input over UDP
public class UDPPacketIO : MonoBehaviour
  {
    private UdpClient Receiver;
    private bool socketsOpen;
    private int localPort;

    void Start()
    {
        //do nothing. init must be called
    }

  	public void init(int localPort){
        LocalPort = localPort;
        socketsOpen = false;
  	}


    ~UDPPacketIO()
    {
        // latest time for this socket to be closed
        if (IsOpen())
        {
            Close();
        }
    }


    // Open a UDP socket and create a UDP receiver.
    public bool Open()
    {
        try
        {
            //Debug.Log("opening udpclient listener on port " + localPort);

            IPEndPoint listenerIp = new IPEndPoint(IPAddress.Any, localPort);
            Receiver = new UdpClient(listenerIp);
            socketsOpen = true;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("cannot open udp client interface at port "+localPort);
            Debug.LogWarning(e);
        }

        return false;
    }

    // Close the socket currently listening.
    public void Close()
    {
        if (Receiver != null)
        {
            Receiver.Close();
            // Debug.Log("UDP receiver closed");
        }
        Receiver = null;
        socketsOpen = false;

    }

    public void OnDisable()
    {
        Close();
    }

    //Returns the state of the UDP socket.
    public bool IsOpen()
    {
      return socketsOpen;
    }


    // Receive a packet of bytes over UDP and returns the numbers of byte read and the address the packet was received from.
    public int ReceivePacket(byte[] buffer, StringBuilder adress)
    {
        if (!IsOpen())
        {
            Open();
        }
           
        if (!IsOpen())
        {
            return 0;
        }

        IPEndPoint iep = new IPEndPoint(IPAddress.Any, localPort);
        byte[] incoming = Receiver.Receive( ref iep );
        adress.Append(iep.Address);
        int count = Math.Min(buffer.Length, incoming.Length);
        System.Array.Copy(incoming, buffer, count);
        return count;
    }


    // The local port you're listening on.
    public int LocalPort
    {
      get
      {
        return localPort;
      }
      set
      {
        localPort = value;
      }
    }
}
