#include "UserSession.h"
#include <packet.pb.h>
#include "SessionManager.h"
#include "RelayServerEventProcessor.h"
#include "PacketHeader.h"
#include "PacketBuilder.h"

UserSession::UserSession()
{
}


UserSession::~UserSession()
{
}

void UserSession::on_received(const boost::system::error_code & err, const void * data, std::size_t size)
{
  if (err)
  {
    shutdown();
    return;
  }

  if ((MaxPacketSize - bufferSize) < size)
  {
    shutdown();
    return;
  }

  memcpy(&packetBuffer[0] + bufferSize, data, size);
  bufferSize += static_cast<int32_t>(size);

  char* bufferReadPos = &packetBuffer[0];

  while (sizeof(packet::PacketHeader) <= bufferSize)
  {
    const packet::PacketHeader* header = reinterpret_cast<packet::PacketHeader*>(bufferReadPos);
    if (header->messageSize < 0)
    {
      shutdown();
      return;
    }

    const int packetSize = header->messageSize + sizeof(packet::PacketHeader);
    if (bufferSize < packetSize)
    {
      return;
    }

    RelayServerEventProcessor::GetInstance().PushEvent(networkId, bufferReadPos, packetSize);

    bufferSize = bufferSize - packetSize;
    bufferReadPos = bufferReadPos + packetSize;
  }

  if (0 < bufferSize)
  {
    memmove(packetBuffer, bufferReadPos, bufferSize);
  }
}

void UserSession::on_sent(const boost::system::error_code & err)
{
}

void UserSession::on_connected()
{
  networkId = ++NetworkIdAllocator;

  SessionManager::GetInstance().RegisterSession(networkId, std::static_pointer_cast<UserSession>(shared_from_this()));

  packet::Connect conn;
  conn.set_network_id(networkId);

  PushEvent(packet::Type::CONNECT, conn);
}

void UserSession::on_disconnected()
{
  SessionManager::GetInstance().UnregisterSession(networkId);

  packet::Disconnect disconn;
  disconn.set_network_id(networkId);

  PushEvent(packet::Type::DISCONNECT, disconn);
}

void UserSession::PushEvent(packet::Type type, const google::protobuf::Message& message)
{
  packet::PacketBuilder builder;
  builder.BuildPacket(type, message);

  RelayServerEventProcessor::GetInstance().PushEvent(networkId, builder.GetPacketPtr(), builder.GetPacketByteSize());
}