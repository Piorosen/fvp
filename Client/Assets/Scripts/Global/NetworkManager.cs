using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class NetworkManager {

    public static NetworkManager Instance = null;
   
    NetClient Network = null;

    public static string ClientName;
    public static long? ClientNetworkId;

    public NetworkManager()
    {
        if (Instance == null)
        {
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
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.LoginReq)
                                .Value
                                .Payload);
    }


    public Queue<PacketInfo> ServerRequest(Vector3 Location)
    {
        Debug.Log("코루틴 시작");
        if (Network != null)
        {
            Debug.Log("널아님");
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
        return Packet.EnterRoomAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.EnterRoomAck)
                                .Value
                                .Payload);
    }

    public Packet.MakeRoomAck MakeRoom(string RoomName, int MaxUser)
    {
        Packet.MakeRoomReq makeRoom = new Packet.MakeRoomReq();

        makeRoom.MaxUserCount = MaxUser;
        makeRoom.RoomName = RoomName;
        Network.Send(Packet.Type.MakeRoomReq, makeRoom);

        return Packet.MakeRoomAck
                     .Parser
                     .ParseFrom(SetTimeOut(5.0f, Packet.Type.MakeRoomAck)
                                .Value
                                .Payload);
    }

    PacketInfo? SetTimeOut(float TimeOut, Packet.Type type)
    {
        while (TimeOut > 0)
        {
            Network.Update();
            PacketInfo info;
            if (Network.TryGetPacket(out info))
            {
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
