using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

public class NetworkManager {

    public static NetworkManager Instance = null;
        
    NetClient Network = null;

    public static string ClientName;
    public static long? ClientNetworkId;

    ConcurrentQueue<PacketInfo> GameQueue = new ConcurrentQueue<PacketInfo>();

    public NetworkManager()
    {
        if (Instance == null)
        {
            Debug.Log("생성");
            Network = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            Instance = this;
        }
    }

    public Packet.LoginAck Login(string PlayerName)
    {
        try
        {
            Network.Connect();
            Packet.LoginReq login = new Packet.LoginReq
            {
                Name = PlayerName
            };
            Network.Send(Packet.Type.LoginReq, login);
        }
        catch (Exception)
        {
            ClientNetworkId = 0;
            Network?.Close();
            Network = null;
            return null;
        }

        return Packet.LoginAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.LoginAck)
                                .Value
                                .Payload);
    }


    public Queue<PacketInfo> ServerRequest(Vector3 Location)
    {
        if (Network != null)
        {
            Packet.MoveReq move = new Packet.MoveReq
            {
                Position = new Packet.Vector3()
                {
                    X = Location.x,
                    Y = Location.y,
                    Z = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond / 1000.0f)
                }
            };

            Network.Send(Packet.Type.MoveReq, move);
            Network.Update();

            PacketInfo info = new PacketInfo();
            Queue<PacketInfo> Result = new Queue<PacketInfo>();
            while (Network.TryGetPacket(out info))
            {
                Result.Enqueue(info);
            }
            return Result;
        }else{
            return null;
        }
    }

    public void LogOut(){
        Packet.Disconnect ack = new Packet.Disconnect();
        ack.NetworkId = ClientNetworkId ?? -1;
        Network.Send(Packet.Type.Disconnect, ack);

    }

    public Packet.GetRoomListAck GetRoomList()
    {
        Packet.GetRoomListReq request = new Packet.GetRoomListReq();
        
        Network.Send(Packet.Type.GetRoomListReq, request);

        return Packet.GetRoomListAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.GetRoomListAck)
                                .Value
                                .Payload);
    }

    public Packet.EnterRoomAck EnterRoom(long RoomId)
    {  
        Packet.EnterRoomReq enterRoom = new Packet.EnterRoomReq();
        enterRoom.RoomId = RoomId;
        Packet.Ack
        
        return Packet.EnterRoomAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.EnterRoomAck)
                                .Value
                                .Payload);
    }

    public Packet.MakeRoomAck MakeRoom(string RoomName, int MaxUser)
    {
        Packet.MakeRoomReq makeRoom = new Packet.MakeRoomReq
        {
            MaxUserCount = MaxUser,
            RoomName = RoomName
        };
        Network.Send(Packet.Type.MakeRoomReq, makeRoom);

        return Packet.MakeRoomAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.MakeRoomAck)
                                .Value
                                .Payload);
    }

    public Packet.ExitRoomUserAck ExitRoom()
    {
        Packet.ExitRoomUserReq exitRoom = new Packet.ExitRoomUserReq
        {
            NetworkId = ClientNetworkId.Value
        };
        Network.Send(Packet.Type.ExitRoomUserReq, exitRoom);

        return Packet.ExitRoomUserAck
                      .Parser
                      .ParseFrom(SetTimeOut(5.0f, Packet.Type.ExitRoomUserAck)
                                 .Value
                                 .Payload);
    }

    public void CastSkill(Vector3 position, Skill skill)
    {
        Packet.CastSkillReq castSkill = new Packet.CastSkillReq
        {
            NetworkId = ClientNetworkId.Value,
            CastPosition = new Packet.Vector3
            {
                X = position.x,
                Y = position.y,
                Z = position.z
            },
            CastDirection = new Packet.Vector3
            {
                X = skill.CastDirection.x,
                Y = skill.CastDirection.y,
                Z = position.z
            },
            SkillId = skill.SkillId
        };
        Network.Send(Packet.Type.CastSkillReq, castSkill);
    }



    PacketInfo? SetTimeOut(float TimeOut, Packet.Type type)
    {
        while (TimeOut > 0)
        {
            Network.Update();
            PacketInfo info;
            if (Network.TryGetPacket(out info))
            {
                Debug.Log(info.Type.ToString());
                if (info.Type == type)
                {
                    return info;
                }
            }
            Thread.Sleep(100);
            TimeOut -= Time.deltaTime + 0.1f;
        }
        return null;
    }

}
