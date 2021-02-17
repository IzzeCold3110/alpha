using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPClient
{
    public Thread readThread;
    private UdpClient client;

    public string udp_server_addr = "127.0.0.1";
    public int port = 9900;

    public bool udp_connected = false;
    public string lastReceivedPacket = "";
    public string allReceivedPackets = ""; // this one has to be cleaned up from time to time

    public void Init()
    {
        client = new UdpClient(udp_server_addr, port);
        readThread = new Thread(new ThreadStart(ReceiveData))
        {
            IsBackground = true
        };
        Debug.Log("starting thread");
        readThread.Start();
    }

    public void StopThread()
    {
        Debug.Log("stopping...");
        if (readThread.IsAlive)
        {
            readThread.Abort();
        }
        client.Close();
    }

    private void ReceiveData()
    {
        
        client.Client.Blocking = true;
        client.Client.ReceiveTimeout = 1000;

        while (true)
        {
            try
            {


                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                Console.WriteLine(">> " + text);
                lastReceivedPacket = text;
                allReceivedPackets += text;
                udp_connected = true;
            }
            catch(SocketException)
            {
                udp_connected = false;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }

    public string GetLatestPacket()
    {
        allReceivedPackets = "";
        return lastReceivedPacket;
    }
}