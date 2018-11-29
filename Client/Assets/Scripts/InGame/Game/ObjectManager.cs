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

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
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
        if (NetworkManager.ClientNetworkId != null){
            try
            {
                Packet.Room room = Packet.Room.Parser.ParseJson(PlayerPrefs.GetString("UserList"));
                PlayerPrefs.DeleteKey("UserList");
                foreach (var item in room.RoomUsers)
                {
                    PlayerManage.AddPlayer(0, new Vector2(item.Position.X, item.Position.Y), item.Name, item.NetworkId);
                }
                PlayerManage.Initialize();
            }catch (Exception){
                NetworkManager.ClientNetworkId = 0;
                PlayerManage.AddPlayer(0, 0, "Offline", 0);
                PlayerManage.Initialize();
                yield break;
            }
        }
        else{
            NetworkManager.ClientNetworkId = 0;
            PlayerManage.AddPlayer(0, 0, "Offline", 0);
            PlayerManage.Initialize();
            yield break;
        }


        while (true)
        {
            var Que = NetworkManager.Instance.ServerRequest(PlayerManage.ClientPlayer.transform.position);
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
                else if (info.Type == Packet.Type.CastSkillAck)
                {
                    Packet.CastSkillAck castSkill = Packet.CastSkillAck.Parser.ParseFrom(info.Payload);

                    if (SkillManager.IsActiveSkill(castSkill.SkillId))
                    {
                        var skill = (SkillManager.SkillInfo[Convert.ToInt32(castSkill.SkillId)] as ActiveSkill);
                        skill.CastPosition = new Vector2
                        {
                            x = castSkill.CastPosition.X,
                            y = castSkill.CastPosition.Y
                        };
                        skill.CastDirection = new Vector2
                        {
                            x = castSkill.CastDirection.X,
                            y = castSkill.CastDirection.Y,
                        };
                        skill.NetworkId = castSkill.NetworkId;
                        PlayerManage.CastSkill(skill);
                    }
                    //
                    //  캐스트 스킬 이벤트 추가해야함.
                    //
                    //
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
