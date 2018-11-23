using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClicked(){
        NetClient client = new NetClient(Global.Network.IPAdress, Global.Network.Port);
        try
        {
            client.Connect();

            Packet.LoginReq login = new Packet.LoginReq
            {
                Name = "Offline",
                NetworkId = 1
            };
            client.Send(Packet.Type.LoginReq, login);
            Debug.Log("Send");

            PacketInfo info = new PacketInfo();
            client.TryGetPacket(out info);

            if (info.Type == Packet.Type.LoginAck)
            {
                Packet.LoginAck ack = Packet.LoginAck.Parser.ParseFrom(info.Payload);
            }
        }
        catch (System.Exception){
            Debug.Log("Error");
        }
        finally
        {
            client.Close();
        }
        

        
    }
}
