using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        var _msg = packet.ReadString();
        var _myId = packet.ReadInt();
        
        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
    }
}