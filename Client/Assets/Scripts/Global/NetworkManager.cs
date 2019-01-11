using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

/// <summary>
/// 싱글톤 개체. 네트워크의 IO 를 담당.
/// </summary>
public class NetworkManager {

    public static NetworkManager Instance = null;
        
    NetClient Network = null;
    public static string ClientName;
    public static long? ClientNetworkId;

    /// <summary>
    /// 네트워크 매니저 초기화 및 싱글톤 적용.
    /// </summary>
    public NetworkManager()
    {
        if (Instance == null)
        {
            Debug.Log("생성");
            Network = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            Instance = this;
        }
    }

    /// <summary>
    /// 로그인을 요청하는 함수. 기본 대기 시간 5.0초 함수 인자를 통하여 시간 조절 가능.
    /// </summary>
    /// <returns>로그인 후 서버에서 받는 데이터 없을 시 null</returns>
    /// <param name="PlayerName">플레이어 이름을 보냅니다. 아직 ID, PW개념은 없습니다.</param>
    public async Task<Packet.LoginAck> Login(string PlayerName, float waitForSecond = 5.0f)
    {
        try
        {
            // 서버가 꺼져 있을 경우. Throw 서버와 연결 시도.
            Network.Connect();
            Packet.LoginReq login = new Packet.LoginReq
            {
                Name = PlayerName
            };
            // 서버에게 데이터를 전달할수 없는 경우 Throw 발.
            Network.Send(Packet.Type.LoginReq, login);
        }
        catch (Exception)
        {
            // 예외가 발생 하면은 서버가 Offline이거나 데이터 네트워크의 불안정이므로 서버와의 연결을 끊습니.
            ClientNetworkId = 0;
            Network?.Close();
            return null;
        }
        // 서버에게 요청한 데이터, Login 데이터가 올 때 까지 대기합니다.
        var payload = await SetTimeOut(waitForSecond, Packet.Type.LoginAck);
        return Packet.LoginAck.Parser.ParseFrom(payload?.Payload);
    }

    /// <summary>
    /// ObjectManager와 연결이 되어서 인게임 데이터와 현재 좌표를 지속적으로 서버에게 보내는 역활입니다.
    /// 그리고 서버에 있는 데이터를 받아옵니다.
    /// </summary>
    /// <returns>현재 서버에게서 받은 데이터 목록입니다.</returns>
    /// <param name="Location">현재 위치를 보냅니다.</param>
    public Queue<PacketInfo> ServerRequest(Vector3 Location)
    {
        if (Network != null)
        {
            // float의 정확도는 10^7 이므로 10^7 의 수로 나머지를 구합니다.
            // 하지 않게 된다면은 비정확도 때문에 오류가 납니다
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

            // 서버에게 현재 위치 데이터를 넘깁니다
            Network.Send(Packet.Type.MoveReq, move);
            // 서버에게 데이터를 받습니다
            Network.Update();

            PacketInfo info = new PacketInfo();
            Queue<PacketInfo> Result = new Queue<PacketInfo>();
            while (Network.TryGetPacket(out info))
            {
                // 서버에 남아 있는 데이터를 전부다 저장합니다
                Result.Enqueue(info);
            }

            return Result;
        }
        else
        {
            // 서버가 꺼져 있다면은 null을 바로 리턴합니다
            return null;
        }
    }

    /// <summary>
    /// 로그아웃
    /// </summary>
    public void LogOut()
    {
        Packet.Disconnect ack = new Packet.Disconnect
        {
            NetworkId = ClientNetworkId ?? -1
        };
        Network.Send(Packet.Type.Disconnect, ack);
    }

    /// <summary>
    /// Close
    /// </summary>
    public void LogOut()
    {
        Network?.Close();
    }

    /// <summary>
    /// 방의 목록을 가져옵니다
    /// </summary>
    /// <returns>방의 목록</returns>
    public async Task<Packet.GetRoomListAck> GetRoomList(float waitForSecond = 5.0f)
    {
        Packet.GetRoomListReq request = new Packet.GetRoomListReq();
        
        Network.Send(Packet.Type.GetRoomListReq, request);

        var payload = await SetTimeOut(waitForSecond, Packet.Type.GetRoomListAck);
        return Packet.GetRoomListAck.Parser.ParseFrom(payload?.Payload);
    }

    /// <summary>
    /// 인게임에 참가합니다
    /// </summary>
    /// <returns>방안에 있는 사람 목록을 리턴합니다</returns>
    /// <param name="RoomId">방의 고유 식별 번호</param>
    /// <param name="waitForSecond">서버와 통신에서 최대 대기 시간입니다</param>
    public async Task<Packet.EnterRoomAck> EnterRoom(long RoomId, float waitForSecond = 2.0f)
    {

        Packet.EnterRoomReq enterRoom = new Packet.EnterRoomReq
        {
            RoomId = RoomId
        };
        Network.Send(Packet.Type.EnterRoomReq, enterRoom);
        var payload = await SetTimeOut(waitForSecond, Packet.Type.EnterRoomAck);
        return Packet.EnterRoomAck.Parser.ParseFrom(payload?.Payload);
    }

    /// <summary>
    /// 방을 만들 때 서버에게 요청합니다.
    /// </summary>
    /// <returns>방을 만들었을 때 방의 Id, 소유자와 같은 데이터를 보냅니다</returns>
    /// <param name="RoomName">방 이름</param>
    /// <param name="MaxUser">최대 입장 가능한 사람수</param>
    public async Task<Packet.MakeRoomAck> MakeRoom(string RoomName, int MaxUser, float waitForSecond = 5.0f)
    {
        Packet.MakeRoomReq makeRoom = new Packet.MakeRoomReq
        {
            MaxUserCount = MaxUser,
            RoomName = RoomName
        };
        Network.Send(Packet.Type.MakeRoomReq, makeRoom);
        var payload = await SetTimeOut(waitForSecond, Packet.Type.MakeRoomAck);
        return Packet.MakeRoomAck.Parser.ParseFrom(payload?.Payload);
    }

    /// <summary>
    /// 방에서 탈주를 요청합니다
    /// </summary>
    /// <returns></returns>
    public async Task<Packet.ExitRoomUserAck> ExitRoom(float waitForSecond = 5.0f)
    {
        Packet.ExitRoomUserReq exitRoom = new Packet.ExitRoomUserReq
        {
            NetworkId = ClientNetworkId.Value
        };
        Network.Send(Packet.Type.ExitRoomUserReq, exitRoom);

        var payload = await SetTimeOut(waitForSecond, Packet.Type.ExitRoomUserAck);
        return Packet.ExitRoomUserAck.Parser.ParseFrom(payload?.Payload);
    }

    /// <summary>
    /// 스킬을 사용시 서버에게 전달합니다.
    /// </summary>
    /// <param name="skill">스킬 데이터를 넘깁니다.</param>
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

    /// <summary>
    /// 서버와 최대 대기 할수 있는 시간을 지정합니다.
    /// </summary>
    /// <returns>패킷 데이터를 리턴합니다. (시간 초과시 널이 리턴됨)</returns>
    /// <param name="waitForSecond">최대 대기 시간</param>
    /// <param name="type">Type.</param>
    private async Task<PacketInfo?> SetTimeOut(float waitForSecond, Packet.Type type)
    {
        while (waitForSecond > 0)
        {
            long t1 = DateTime.UtcNow.Ticks;
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
            long t2 = DateTime.UtcNow.Ticks;
            waitForSecond -= ((t2 - t1) / TimeSpan.TicksPerMillisecond) / 1000.0f;
        }
        return null;
    }

}
