#pragma once

#include <google/protobuf/message.h>
#include <packet.pb.h>
#include "EventProcessor.h"
#include "RoomUser.h"
#include "Room.h"
#include "PacketBuilder.h"
#include "PacketDataSerializer.h"


class RelayServerEventProcessor : public EventProcessor, public Singleton<RelayServerEventProcessor>
{
public:

	friend class Singleton<RelayServerEventProcessor>;

	void HandleConnect(int64_t networkId, const packet::Connect& message);
	void HandleDisconnect(int64_t networkId, const packet::Disconnect& message);
	void HandleLoginReq(int64_t networkId, const packet::LoginReq& message);
	void HandleMoveReq(int64_t networkId, const packet::MoveReq& message);
	void HandleEnterRoomReq(int64_t networkId, const packet::EnterRoomReq& message);
	void HandleMakeRoomReq(int64_t networkId, const packet::MakeRoomReq& message);
	void HandleGetRoomListReq(int64_t networkId, const packet::GetRoomListReq& message);
	void HandleMoveRoomUserReq(int64_t networkId, const packet::MoveRoomUserReq& message);
	void HandleExitRoomUserReq(int64_t networkId, const packet::ExitRoomUserReq& message);
	void HandleCastSkillReq(int64_t networkId, const packet::CastSkillReq& message);
	void HandleCastSkillHitReq(int64_t networkId, const packet::CastSkillHitReq& message);

	void Send(int64_t networkId, packet::Type type, const google::protobuf::Message& message);
	void Send(int64_t networkId, const void* data, int dataSize);
	void SendAll(packet::Type type, const google::protobuf::Message& message);
	void SendToRoomUsers(int64_t roomId, const void* data, int dataSize);
	void SendToRoomUsers(int64_t roomId, packet::Type type, const google::protobuf::Message& message);

	bool IsLoggedInUser(int64_t networkId) const;
	void AddLoginUser(int64_t networkId, RoomUser user);
	void RemoveLoggedInUser(int64_t networkId);
	RoomUser& GetLoginUser(int64_t networkId);
	Room* GetUserRoom(int64_t networkId);
	Room* GetRoom(int64_t roomId);
	int64_t GetUserRoomId(int64_t userNetworkId) const;

protected:

	virtual void Start();
	virtual void Update();
	virtual void HandleDefaultEvent(int64_t networkId, const void* src, int size);

private:

	std::unordered_map<int64_t, RoomUser> users;
	std::unordered_map<int64_t, int64_t> userRoomIdInfos;
	std::unordered_map<int64_t, Room> rooms;
	int64_t lastRoomId = 0;
	packet::PacketBuilder packetBuilder;
	PacketDataSerializer packetDataSerializer;
};