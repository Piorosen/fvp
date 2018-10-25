#include "UserSession.h"
#include <packet.pb.h>
#include "EventProcessor.h"
#include "PacketHeader.h"

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

  if (MaxPacketSize - currentPacketSize < size)
  {
    shutdown();
    return;
  }

  memcpy(packetBuffer + currentPacketSize, data, size);
  currentPacketSize += static_cast<int32_t>(size);

  if (sizeof(packet::PacketHeader) <= currentPacketSize)
  {
	  const packet::PacketHeader* header = reinterpret_cast<packet::PacketHeader*>(packetBuffer);
    if (header->packetSize < 0)
    {
      shutdown();
      return;
    }
	  if (currentPacketSize <= header->packetSize)
	  {// 패킷이 완성됐다면
		  const auto remainingSize = currentPacketSize - header->packetSize;
		  
      EventProcessor::GetInstance().PushEvent(packetBuffer + sizeof(packet::PacketHeader), header->packetSize);
		  
      memmove(packetBuffer, packetBuffer + header->packetSize, remainingSize);
		  currentPacketSize = remainingSize;
	  }
  }
}

void UserSession::on_sent(const boost::system::error_code & err)
{
}

void UserSession::on_connected()
{
  networkId = ++NetworkIdAllocator;

  packet::Connect conn;
  conn.set_network_id(networkId);

  if (SerializeToPacketBuffer(conn))
  {
    EventProcessor::GetInstance().PushEvent(packetBuffer, currentPacketSize);
  }
}

void UserSession::on_disconnected()
{
	packet::Disconnect disconn;
	disconn.set_network_id(networkId);

	if (SerializeToPacketBuffer(disconn))
	{
		EventProcessor::GetInstance().PushEvent(packetBuffer, currentPacketSize);
	}
}

bool UserSession::SerializeToPacketBuffer(const google::protobuf::Message& message)
{
  const bool res = message.SerializeToArray(packetBuffer, sizeof(packetBuffer));
  currentPacketSize = res ? message.ByteSize() : 0;
  return res;
}