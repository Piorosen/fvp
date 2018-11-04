#include "RelayServerEventProcessor.h"
#include "SessionManager.h"
#include "PacketBuilder.h"

void RelayServerEventProcessor::HandleLogin(int64_t networkId, const packet::LoginReq & message)
{
}

void RelayServerEventProcessor::HandleMove(int64_t networkId, const packet::MoveReq & message)
{
}

void RelayServerEventProcessor::HandleDefaultEvent(int64_t networkId, const void* src, int size)
{
}

void RelayServerEventProcessor::HandleConnect(int64_t networkId, const packet::Connect & message)
{
}

void RelayServerEventProcessor::HandleDisconnect(int64_t networkId, const packet::Disconnect & message)
{
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

void RelayServerEventProcessor::Update()
{
  EventProcessor::Update();
}