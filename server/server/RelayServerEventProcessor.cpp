#include "RelayServerEventProcessor.h"
#include "SessionManager.h"
#include "PacketBuilder.h"

void RelayServerEventProcessor::HandleLogin(int64_t networkId, const packet::LoginReq & message)
{
  if (IsLoggedInUser(networkId))
  {
	return;
  }

  RoomUser user;
  user.name = message.name();
  user.networkId = networkId;
  AddLoginUser(networkId, std::move(user));

  packet::LoginAck ack;
  ack.set_name(message.name());
  ack.set_network_id(networkId);
  
  SendAll(packet::Type::LOGIN_ACK, ack);
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