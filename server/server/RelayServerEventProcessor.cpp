#include "RelayServerEventProcessor.h"
#include "SessionManager.h"
#include "PacketBuilder.h"

void RelayServerEventProcessor::HandleLogin(int64_t networkId, const packet::LoginReq & message)
{
  if (IsLoggedInUser(networkId))
  {
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

void RelayServerEventProcessor::HandleMove(int64_t networkId, const packet::MoveReq & message)
{
  if (!IsLoggedInUser(networkId))
  {
	return;
  }

  RoomUser& user = GetLoginUser(networkId);
  user.position = message.position();

  packet::MoveAck ack;
  ack.set_network_id(networkId);
  *ack.mutable_position() = message.position();

  SendAll(packet::Type::MOVE_ACK, ack);
}

void RelayServerEventProcessor::HandleDefaultEvent(int64_t networkId, const void* src, int size)
{
}

void RelayServerEventProcessor::HandleConnect(int64_t networkId, const packet::Connect & message)
{
}

void RelayServerEventProcessor::HandleDisconnect(int64_t networkId, const packet::Disconnect & message)
{
  if (IsLoggedInUser(networkId))
  {
	RemoveLoggedInUser(networkId);
  }
}

void RelayServerEventProcessor::Start()
{
  BindHandler(packet::Type::LOGIN_REQ, &RelayServerEventProcessor::HandleLogin);
  BindHandler(packet::Type::MOVE_REQ, &RelayServerEventProcessor::HandleMove);
  BindHandler(packet::Type::DISCONNECT, &RelayServerEventProcessor::HandleDisconnect);
  BindHandler(packet::Type::CONNECT, &RelayServerEventProcessor::HandleConnect);
}

void RelayServerEventProcessor::Send(int64_t networkId, packet::Type type, const google::protobuf::Message& message)
{
  auto session = SessionManager::GetInstance().GetSession(networkId);
  if (session)
  {
	packet::PacketBuilder builder;
	if (builder.BuildPacket(type, message))
	{
	  session->send(builder.GetPacketPtr(), builder.GetPacketByteSize());
	}
  }
}

void RelayServerEventProcessor::SendAll(packet::Type type, const google::protobuf::Message& message)
{
  packet::PacketBuilder builder;
  if (builder.BuildPacket(type, message))
  {
	for (auto& user : users)
	{
	  auto session = SessionManager::GetInstance().GetSession(user.second.networkId);
	  if (session)
	  {
		session->send(builder.GetPacketPtr(), builder.GetPacketByteSize());
	  }
	}
  }
}

void RelayServerEventProcessor::Update()
{
  EventProcessor::Update();
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