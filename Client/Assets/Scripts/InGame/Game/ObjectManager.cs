using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance = null;

    public InGameCamera Camera;
    public PlayerManager PlayerManage;

    NetClient Client = null;

    public float Ping;
    public Text text;

    void OnDestroy()
    {
        
        if (Client != null)
        {
            Client.Close();
            Client.Update();
        }
    }

    void Awake() {
        if (Instance == null)
        {
            Instance = this;

            Client = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            try
            {
                text.text = Ping.ToString();
                Client.Connect();
                Packet.LoginReq login = new Packet.LoginReq
                {
                    Name = PlayerPrefs.GetString("PlayerName")
                };
                PlayerManage.ClientName = login.Name;
                Client.Send(Packet.Type.LoginReq, login);

                StartCoroutine(ServerPlayer());
                StartCoroutine(ServerRequest());
            }
            catch (Exception){
                PlayerManager.ClientNetworkId = 0;
                PlayerManage.ClientName = "Offline";
                PlayerManage.AddPlayer(0, 0, "Offline", 0);
                PlayerManage.Initialize();

                Client.Close();
                Client = null;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    

	void Start () {

    }

    bool Login = false;
    IEnumerator ServerPlayer()
    {
        while (true)
        {
            Client.Update();
            PacketInfo info;
            
            if (Client.TryGetPacket(out info))
            {
                if (info.Type == Packet.Type.LoginAck)
                {
                    Login = true;
                    Packet.LoginAck enter = Packet.LoginAck.Parser.ParseFrom(info.Payload);
                    if (enter.Name == PlayerManage.ClientName) {
                        PlayerManager.ClientNetworkId = enter.NetworkId;
                    }
                    PlayerManage.AddPlayer(0, 0, enter.Name, enter.NetworkId);

                    PlayerManage.Initialize();

                    for (int i = 0; i < enter.Users.Count; i++)
                    {
                        if (enter.Users[i].NetworkId != PlayerManager.ClientNetworkId)
                            PlayerManage.AddPlayer(0, 0, enter.Users[i].Name, enter.Users[i].NetworkId);
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f / 30);
        }
        yield return null;
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
                if (Login != true)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                Vector3 location = new Vector3();

                location = PlayerManage.ClientPlayer.transform.position;
                Packet.MoveReq move = new Packet.MoveReq
                {
                    Position = new Packet.Vector3()
                    {
                        X = location.x,
                        Y = location.y,
                        Z = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond / 1000.0f) % 100000
                    }
                };
                
                Client.Send(Packet.Type.MoveReq, move);
                Client.Update();

                PacketInfo info = new PacketInfo();
                while (Client.TryGetPacket(out info))
                {
                    if (info.Type == Packet.Type.MoveAck)
                    {
                        Packet.MoveAck moveAck = Packet.MoveAck.Parser.ParseFrom(info.Payload);
                        Vector4 data = new Vector4(moveAck.Position.X, moveAck.Position.Y, moveAck.Position.Z)
                        {
                            w = moveAck.NetworkId
                        };
                        PlayerManage.MovementQueue.Enqueue(data);
                    }
                    else if (info.Type == Packet.Type.EnterNewUserAck)
                    {
                        Packet.EnterNewUserAck enter = Packet.EnterNewUserAck.Parser.ParseFrom(info.Payload);
                        if (enter.NewUser.NetworkId != PlayerManager.ClientNetworkId)
                        {
                            PlayerManage.AddPlayer(0, 0, enter.NewUserName, enter.NewUser.NetworkId);
                        }
                    }else if (info.Type == Packet.Type.Disconnect)
                    {
                        Packet.Disconnect disconnect = Packet.Disconnect.Parser.ParseFrom(info.Payload);
                        PlayerManage.DelPlayer(disconnect.NetworkId);
                    }
                    else
                    {
                      Debug.Log(info.Type);
                    }
                }
                yield return new WaitForSeconds(Ping / 1000.0f);
            }
        }
        Debug.Log("코루틴 종료");
    }


    private void FixedUpdate()
    {
        if (PlayerManager.ClientNetworkId != null)
        {
            Camera.Target = PlayerManage.LocationAverage;
            Camera.NeedSize = PlayerManage.LocationCamera;
        }
    }
}
