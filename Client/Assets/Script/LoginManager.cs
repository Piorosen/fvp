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
        NetClient client = new NetClient("127.0.0.1", 16333);
        try
        {
            client.Connect();
            Packet.Login login = new Packet.Login();
            login.NetworkId = 0;
            login.Name = this.GetHashCode().ToString();
            client.Send(Packet.Type.Login, login);
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
