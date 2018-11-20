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
                NetworkId = 0,
                Name = this.GetHashCode().ToString()
            };
            client.Send(Packet.Type.LoginReq, login);
            Debug.Log("Send");

            PacketInfo info = new PacketInfo();
            client.TryGetPacket(out info);

            if (info.Type == Packet.Type.LoginAck)
            {
                Debug.Log("Respond");
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
