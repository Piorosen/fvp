#include "RelayServerEventProcessor.h"
#include "SessionManager.h"
#include "PacketBuilder.h"
#include "PacketHeader.h"
#include "PacketDataSerializer.h"

void RelayServerEventProcessor::Start()
{
	
	BindHandler(packet::Type::CONNECT, &RelayServerEventProcessor::HandleConnect);
	BindHandler(packet::Type::DISCONNECT, &RelayServerEventProcessor::HandleDisconnect);
	BindHandler(packet::Type::LOGIN_REQ, &RelayServerEventProcessor::HandleLoginReq);
	//BindHandler(packet::Type::LOGOUT_REQ, &RelayServerEventProcessor::HandleLogoutReq);
	BindHandler(packet::Type::MOVE_REQ, &RelayServerEventProcessor::HandleMoveReq);

	BindHandler(packet::Type::MAKE_ROOM_REQ, &RelayServerEventProcessor::HandleMakeRoomReq);
	BindHandler(packet::Type::GET_ROOM_LIST_REQ, &RelayServerEventProcessor::HandleGetRoomListReq);
	BindHandler(packet::Type::ENTER_ROOM_REQ, &RelayServerEventProcessor::HandleEnterRoomReq);
	BindHandler(packet::Type::MOVE_ROOM_USER_REQ, &RelayServerEventProcessor::HandleMoveRoomUserReq);
	BindHandler(packet::Type::EXIT_ROOM_USER_REQ, &RelayServerEventProcessor::HandleExitRoomUserReq);
	BindHandler(packet::Type::CAST_SKILL_REQ, &RelayServerEventProcessor::HandleCastSkillReq);
}

void RelayServerEventProcessor::HandleCastSkillReq(int64_t networkId, const packet::CastSkillReq& message)
{
	const int64_t roomId = GetUserRoomId(networkId);

	packet::CastSkillAck ack;
	ack.set_network_id(networkId);
	ack.set_skill_id(message.network_id());
	*ack.mutable_cast_position() = message.cast_position();
	*ack.mutable_cast_direction() = message.cast_direction();

	SendToRoomUsers(roomId, packet::Type::CAST_SKILL_ACK, ack);
}

void RelayServerEventProcessor::HandleExitRoomUserReq(int64_t networkId, const packet::ExitRoomUserReq& message)
{
	const int64_t roomId = GetUserRoomId(networkId);

	Room* room = GetUserRoom(networkId);
	if (room == nullptr)
	{
		return;
	}

	if (!room->RemoveUser(networkId))
	{
		return;
	}

	packet::ExitRoomUserAck ack;

	ack.set_network_id(networkId);

	SendToRoomUsers(roomId, packet::Type::EXIT_ROOM_USER_ACK, ack);
}

void RelayServerEventProcessor::HandleMoveReq(int64_t networkId, const packet::MoveReq & message)
{
	if (!IsLoggedInUser(networkId))
	{// 로그인 안 함
		return;
	}

	Room* room = GetUserRoom(networkId);
	if (room == nullptr)
	{
		return;
	}

	packet::MoveAck ack;
	ack.set_network_id(networkId);
	*ack.mutable_position() = message.position();

	auto roomUsers = room->GetUserGroup();

	for (int i = 0; i != roomUsers->GetUserCount(); ++i)
	{
		RoomUser* user = roomUsers->GetUser(i);

		Send(user->networkId, packet::Type::MOVE_ACK, ack);
	}
}

void RelayServerEventProcessor::HandleMoveRoomUserReq(int64_t networkId, const packet::MoveRoomUserReq& message)
{
	const int64_t roomId = GetUserRoomId(networkId);

	Room* room = GetUserRoom(networkId);
	if (room == nullptr)
	{
		return;
	}

	RoomUser* roomUser = room->GetUserGroup()->FindUserByNetworkId(networkId);
	if (roomUser == nullptr)
	{
		return;
	}

	packet::MoveRoomUserAck ack;
	ack.set_network_id(networkId);
	*ack.mutable_position() = roomUser->position;

	SendToRoomUsers(roomId, packet::Type::MOVE_ROOM_USER_ACK, ack);
}

void RelayServerEventProcessor::HandleEnterRoomReq(int64_t networkId, const packet::EnterRoomReq& message)
{
	if (!IsLoggedInUser(networkId))
	{// 로그인 안 함
		return;
	}

	if (GetUserRoom(networkId) != nullptr)
	{// 이미 방에 있음
		return;
	}

	const int64_t roomId = message.room_id();
	Room* room = GetRoom(roomId);
	if (room == nullptr)
	{// 없는 방
		return;
	}

	if (room->IsFull())
	{// 꽉찬 방
		return;
	}

	// 방에 유저 추가
	room->EnterRoom(networkId);
	
	UserGroup* userGroup = room->GetUserGroup();

	{// 기존 유저에게 새로운 유저의 입장을 알린다.

		RoomUser* newUser = userGroup->FindUserByNetworkId(networkId);
		if (newUser == nullptr)
		{
			return;
		}

		packet::EnterNewUserAck ack;

		packetDataSerializer.Serialize(*newUser, *ack.mutable_new_user());

		for (int i = 0; i < userGroup->GetUserCount(); ++i)
		{
			auto user = userGroup->GetUser(i);
			if (user->networkId != networkId)
			{
				Send(networkId, packet::Type::ENTER_NEW_USER_ACK, ack);
			}
		}
	}
	
	{// 새로 들어온 유저에게 방 정보를 알린다.
		packet::EnterRoomAck ack;

		ack.set_room_id(roomId);
		packetDataSerializer.Serialize(*room, *ack.mutable_room());

		SendToRoomUsers(roomId, packet::Type::ENTER_ROOM_ACK, ack);
	}
}

void RelayServerEventProcessor::HandleMakeRoomReq(int64_t networkId, const packet::MakeRoomReq& message)
{
	if (!IsLoggedInUser(networkId))
	{// 로그인 안 함
		return;
	}

	if (GetUserRoom(networkId) != nullptr)
	{// 이미 방에 있음
		return;
	}

	if (message.max_user_count() < 1)
	{
		return;
	}

	const int64_t roomId = ++lastRoomId;

	Room room(roomId);
	room.SetRoomName(message.room_name());
	room.SetMaxUserCount(message.max_user_count());
	room.EnterRoom(networkId);

	packet::MakeRoomAck ack;
	packetDataSerializer.Serialize(room, *ack.mutable_room());

	rooms.emplace(roomId, std::move(room));
	
	Send(networkId, packet::Type::MAKE_ROOM_ACK, ack);
}

void RelayServerEventProcessor::HandleGetRoomListReq(int64_t networkId, const packet::GetRoomListReq& message)
{
	if (!IsLoggedInUser(networkId))
	{// 로그인 안 함
		return;
	}

	packet::GetRoomListAck ack;

	PacketDataSerializer serializer;

	for (const auto& item : rooms)
	{
		const int64_t roomId = item.first;
		const Room& room = item.second;

		packet::Room* addRoom = ack.add_rooms();

		serializer.Serialize(room, *addRoom);
	}
	
	Send(networkId, packet::Type::GET_ROOM_LIST_ACK, ack);
}

void RelayServerEventProcessor::HandleLoginReq(int64_t networkId, const packet::LoginReq & message)
{
  if (IsLoggedInUser(networkId))
  {// 이미 로그인 됨
		return;
  }

  RoomUser newUser;
  newUser.name = message.name();
  newUser.networkId = networkId;

  AddLoginUser(networkId, std::move(newUser));
  
  packet::LoginAck loginAck;
  loginAck.set_name(message.name());
  loginAck.set_network_id(networkId);
  for (auto& item : users)
  {
		auto& user = item.second;
		auto addUser = loginAck.add_users();
		*(addUser->mutable_position()) = user.position;
		addUser->set_network_id(user.networkId);
		addUser->set_name(user.name);
  }
  Send(networkId, packet::Type::LOGIN_ACK, loginAck);
  
  RoomUser& user = GetLoginUser(networkId);
  packet::EnterNewUserAck enterAck;
  enterAck.set_new_user_name(user.name);
  auto mutNewUser = enterAck.mutable_new_user();
  mutNewUser->set_network_id(networkId);
  *mutNewUser->mutable_position() = user.position;

  SendAll(packet::Type::ENTER_NEW_USER_ACK, enterAck);
}

void RelayServerEventProcessor::HandleDefaultEvent(int64_t networkId, const void* src, int size)
{
}

void RelayServerEventProcessor::HandleConnect(int64_t networkId, const packet::Connect & message)
{
}

void RelayServerEventProcessor::HandleDisconnect(int64_t networkId, const packet::Disconnect & message)
{
	const int64_t roomId = GetUserRoomId(networkId);

	Room* room = GetRoom(roomId);
	if (room == nullptr)
	{
		packet::ExitRoomUserReq req;
		
		req.set_network_id(networkId);

		HandleExitRoomUserReq(networkId, req);
	}

  if (IsLoggedInUser(networkId))
  {
		RemoveLoggedInUser(networkId);

		packet::LogoutAck ack;

		ack.set_network_id(networkId);

		SendAll(packet::Type::LOGOUT_ACK, ack);
  }
}

void RelayServerEventProcessor::Send(int64_t networkId, const void* data, int dataSize)
{
	auto session = SessionManager::GetInstance().GetSession(networkId);
	if (session)
	{
		session->send(data, dataSize);
	}
}

void RelayServerEventProcessor::Send(int64_t networkId, packet::Type type, const google::protobuf::Message& message)
{
  auto session = SessionManager::GetInstance().GetSession(networkId);
  if (session)
  {
		if (packetBuilder.BuildPacket(type, message))
		{
			session->send(packetBuilder.GetPacketPtr(), packetBuilder.GetPacketByteSize());
		}
  }
}

void RelayServerEventProcessor::SendAll(packet::Type type, const google::protobuf::Message& message)
{
  if (packetBuilder.BuildPacket(type, message))
  {
		for (auto& user : users)
		{
			auto session = SessionManager::GetInstance().GetSession(user.second.networkId);
			if (session)
			{
				session->send(packetBuilder.GetPacketPtr(), packetBuilder.GetPacketByteSize());
			}
		}
  }
}

void RelayServerEventProcessor::SendToRoomUsers(int64_t roomId, const void* data, int dataSize)
{
	Room* room = GetRoom(roomId);
	if (room == nullptr)
	{
		return;
	}

	UserGroup* userGroup = room->GetUserGroup();
	for (int i = 0; i < userGroup->GetUserCount(); ++i)
	{
		auto user = userGroup->GetUser(i);
		Send(user->networkId, packetBuilder.GetPacketPtr(), packetBuilder.GetPacketByteSize());
	}
}

void RelayServerEventProcessor::SendToRoomUsers(int64_t roomId, packet::Type type, const google::protobuf::Message& message)
{
	if (packetBuilder.BuildPacket(type, message))
	{
		SendToRoomUsers(roomId, packetBuilder.GetPacketPtr(), packetBuilder.GetPacketByteSize());
	}
}

void RelayServerEventProcessor::Update()
{
  /*for (auto& item : rooms)
  {
    item.second.Update();
  }*/

  EventProcessor::Update();
}

Room* RelayServerEventProcessor::GetUserRoom(int64_t networkId)
{
	auto i = userRoomIdInfos.find(networkId);
	if(i != userRoomIdInfos.end())
	{
		auto roomIt = rooms.find(i->second);
		if (roomIt != rooms.end())
		{
			return &(roomIt->second);
		}
	}
	return nullptr;
}

int64_t RelayServerEventProcessor::GetUserRoomId(int64_t userNetworkId) const
{
	auto i = userRoomIdInfos.find(userNetworkId);
	return i == userRoomIdInfos.end() ? -1 : i->second;
}

Room * RelayServerEventProcessor::GetRoom(int64_t roomId)
{
	auto i = rooms.find(roomId);
	return i != rooms.end() ? &(i->second) : nullptr;
}

bool RelayServerEventProcessor::IsLoggedInUser(int64_t networkId) const
{
  auto i = users.find(networkId);
  return i != users.end();
}

void RelayServerEventProcessor::AddLoginUser(int64_t networkId, RoomUser user)
{
  users[networkId] = std::move(user);
}

RoomUser& RelayServerEventProcessor::GetLoginUser(int64_t networkId)
{
  return users[networkId];
}

void RelayServerEventProcessor::RemoveLoggedInUser(int64_t networkId)
{
  users.erase(networkId);
}