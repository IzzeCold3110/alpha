using UnityEngine;

public class StartUDP : MonoBehaviour
{
    public UDPClient client;

    public void Start()
    {
        client = new UDPClient();
        client.Init();
    }

    public void Update()
    {
        if(Input.GetKey("q"))
        {
            if(client != null)
            {
                if(client.readThread.IsAlive == true)
                {
                    client.StopThread();
                }
            }
        }
    }
}
