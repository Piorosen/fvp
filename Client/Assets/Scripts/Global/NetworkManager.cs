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

    public NetworkManager()
    {
        if (Instance == null)
        {
            Debug.Log("생성");
            Network = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            Instance = this;
        }
    }

    public async Task<Packet.LoginAck> Login(string PlayerName)
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
        var payload = await SetTimeOut(5.0f, Packet.Type.LoginAck);
        return Packet.LoginAck.Parser.ParseFrom(payload?.Payload);
    }


    public Queue<PacketInfo> ServerRequest(Vector3 Location)
    {
        if (Network != null)
        {
            float t = Convert.ToSingle((DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond) % 10000000);
            t /= 1000f;
            Packet.MoveReq move = new Packet.MoveReq
            {
                Position = new Packet.Vector3()
                {
                    X = Location.x,
                    Y = Location.y,
                    Z = t
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
        Packet.Disconnect ack = new Packet.Disconnect
        {
            NetworkId = ClientNetworkId ?? -1
        };
        Network.Send(Packet.Type.Disconnect, ack);

    }

    public async Task<Packet.GetRoomListAck> GetRoomList()
    {
        Packet.GetRoomListReq request = new Packet.GetRoomListReq();
        
        Network.Send(Packet.Type.GetRoomListReq, request);

        var payload = await SetTimeOut(5.0f, Packet.Type.GetRoomListAck);
        return Packet.GetRoomListAck.Parser.ParseFrom(payload?.Payload);
    }

    public async Task<Packet.EnterRoomAck> EnterRoom(long RoomId)
    {

        Packet.EnterRoomReq enterRoom = new Packet.EnterRoomReq
        {
            RoomId = RoomId
        };
        Network.Send(Packet.Type.EnterRoomReq, enterRoom);
        var payload = await SetTimeOut(1.0f, Packet.Type.EnterRoomAck);
        return Packet.EnterRoomAck.Parser.ParseFrom(payload?.Payload);
    }

    public async Task<Packet.MakeRoomAck> MakeRoom(string RoomName, int MaxUser)
    {
        Packet.MakeRoomReq makeRoom = new Packet.MakeRoomReq
        {
            MaxUserCount = MaxUser,
            RoomName = RoomName
        };
        Network.Send(Packet.Type.MakeRoomReq, makeRoom);
        var payload = await SetTimeOut(5.0f, Packet.Type.MakeRoomAck);
        return Packet.MakeRoomAck
                     .Parser
                     .ParseFrom(payload?.Payload);
    }

    public async Task<Packet.ExitRoomUserAck> ExitRoom()
    {
        Packet.ExitRoomUserReq exitRoom = new Packet.ExitRoomUserReq
        {
            NetworkId = ClientNetworkId.Value
        };
        Network.Send(Packet.Type.ExitRoomUserReq, exitRoom);

        var payload = await SetTimeOut(5.0f, Packet.Type.ExitRoomUserAck);
        return Packet.ExitRoomUserAck
                      .Parser
                      .ParseFrom(payload?.Payload);
    }

    public void CastSkill(NetworkSkill skill)
    {
        Packet.CastSkillReq castSkill = new Packet.CastSkillReq
        {
            NetworkId = ClientNetworkId.Value,
            SkillId = skill.SkillId,
            CastPosition = new Packet.Vector3
            {
                X = skill.CastPosition.x,
                Y = skill.CastPosition.y,
                Z = -1
            },
            CastDirection = new Packet.Vector3
            {
                X = skill.CastDirection.x,
                Y = skill.CastDirection.y,
                Z = -1
            }
        };
        Network.Send(Packet.Type.CastSkillReq, castSkill);
    }


    async Task<PacketInfo?> SetTimeOut(float TimeOut, Packet.Type type)
    {
        while (TimeOut > 0)
        {
            Network.Update();
            PacketInfo info;
            while (Network.TryGetPacket(out info))
            {
                Debug.Log(info.Type.ToString());
                if (info.Type == type)
                {
                    return await Task.FromResult(info);
                }
            }
            await Task.Delay(100);
            TimeOut -= 0.1f;
        }
        return null;
    }

}
