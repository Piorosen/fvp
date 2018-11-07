using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;

    public GameObject Camera;
    public MovementManager Movement;

    NetClient Client = null;


    void Awake() {
        if (Instance == null)
        {
            Instance = this;

            Client = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            try
            {
                Client.Connect();
            }catch (Exception){
                Client.Close();
                Client = null;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        StartCoroutine("ServerRequest");
    }

    /// <summary>
    /// 이것을 이제 개인 클라이언트에게 값을 던져줘야함.
    /// </summary>
    /// 

    IEnumerator ServerRequest()
    {
        Debug.Log("코루틴 시작");
        if (Client != null)
        {
            Debug.Log("널아님");
            while (true)
            {
                Vector3 location = new Vector3();

                location = Movement.ClientPlayer
                    .GetComponent<Character>().transform.position;

                Packet.MoveReq move = new Packet.MoveReq
                {
                    Position = new Packet.Vector3()
                    {
                        X = location.x,
                        Y = location.y,
                        Z = location.z
                    }
                };

                Client.Send(Packet.Type.MoveReq, move);
                Client.Update();

                PacketInfo info = new PacketInfo();
                if (Client.TryGetPacket(out info))
                {
                    
                    if (info.Type == Packet.Type.MoveAck)
                    {
                        Packet.MoveAck moveAck = Packet.MoveAck.Parser.ParseFrom(info.Payload);
                        Debug.Log("Network : " + moveAck.NetworkId + " Pos : " + moveAck.Position.ToString());
                    }
                    
                }

                yield return new WaitForSeconds(0.1f);

            }
        }
        Debug.Log("코루틴 종료");
    }


    private void FixedUpdate()
    {

        Movement.ClientMove();

        var Character = Camera.GetComponent<InGameCamera>();

        Character.Target = Movement.LocationAverage;

        Character.NeedSize = Movement.LocationCamera;
    }




    // Update is called once per frame
    void Update()
    {
		
	}
}
