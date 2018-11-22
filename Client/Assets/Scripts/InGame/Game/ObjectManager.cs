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
    NetworkManager NetworkManage;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            NetworkManage = NetworkManager.Instance ?? new NetworkManager();
            StartCoroutine(ServerRequest());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 이것을 이제 개인 클라이언트에게 값을 던져줘야함.
    /// </summary>
    IEnumerator ServerRequest(){
        var login = NetworkManage.Login("Offline");
        if (login != null){
            foreach (var item in login.Users){
                PlayerManage.AddPlayer(0, new Vector2(item.Position.X, item.Position.Y), item.Name, item.NetworkId);
            }
        }
        else{
            PlayerManage.AddPlayer(0, 0, "Offline", 0);
            PlayerManage.Initialize();
            yield break;
        }


        while (true)
        {
            var Que = NetworkManage.ServerRequest(PlayerManage.ClientPlayer.transform.position);
            while (Que.Count != 0)
            {
                var info = Que.Dequeue();
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
                    if (enter.NewUser.NetworkId != NetworkManager.ClientNetworkId)
                    {
                        PlayerManage.AddPlayer(0, 0, enter.NewUserName, enter.NewUser.NetworkId);
                    }
                }
                else if (info.Type == Packet.Type.Disconnect)
                {
                    Packet.Disconnect disconnect = Packet.Disconnect.Parser.ParseFrom(info.Payload);
                    PlayerManage.DelPlayer(disconnect.NetworkId);
                }
                else
                {
                    Debug.Log(info.Type);
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }


    private void FixedUpdate()
    {
        if (NetworkManager.ClientNetworkId != null)
        {
            Camera.Target = PlayerManage.LocationAverage;
            Camera.NeedSize = PlayerManage.LocationCamera;
        }
    }
}
