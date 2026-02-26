using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.tcp.SendData(packet);
    }
    
    public static void WelcomeReceived()
    {
        using (var packet = new Packet((int)PacketType.welcomeReceived))
        {
            packet.Write(Client.instance.myId);
            packet.Write("Hello Server!");
            SendTCPData(packet);
        }
    }
}
