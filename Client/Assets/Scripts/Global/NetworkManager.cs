using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager Instance = null;

   
    NetClient Network = null;
    bool LoginCheck = false;

    public static string ClientName;
    public static long? ClientNetworkId;
    void OnDestroy()
    {
        if (Network != null)
        {
            Network.Close();
        }
    }
        
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Network = new NetClient(Global.Network.IPAdress, Global.Network.Port);
        }
        else
        {
            Destroy(this);
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
            Network.Close();
            Network = null;
        }
        float TimeOut = 5.0f;
        while (TimeOut > 0)
        {
            Network.Update();
            PacketInfo info;
            if (Network.TryGetPacket(out info))
            {
                if (info.Type == Packet.Type.LoginAck)
                {
                    Packet.LoginAck enter = Packet.LoginAck.Parser.ParseFrom(info.Payload);
                    LoginCheck = true;
                    ClientNetworkId = enter.NetworkId;
                    ClientName = enter.Name;
                    return enter;
                }
            }
            Thread.Sleep(100);
            TimeOut -= Time.deltaTime + 0.1f;
        }
        return null;
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
        Packet.EnterRoom enterRoom = new Packet.EnterRoom();
        enterRoom.NetworkId = ClientNetworkId.Value;
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
