#pragma once

#include <google/protobuf/message.h>
#include <packet.pb.h>
#include "PacketHeader.h"

namespace packet
{
  class PacketBuilder
  {
  public:

    PacketBuilder()
    {
      bufferPtr = &staticBuffer[0];
      bufferSize = static_cast<int>(sizeof(staticBuffer));
    }

    PacketBuilder(void* buffer, int bufferSize) :
      bufferPtr(static_cast<char*>(buffer)),
      bufferSize(bufferSize)
    {
    }
    
    bool BuildPacket(packet::Type type, const google::protobuf::Message& message)
    {
      const int messageSize = message.ByteSize();
      const int packetSize = static_cast<int>(sizeof(packet::PacketHeader)) + messageSize;
      if (bufferSize < packetSize)
      {
        return false;
      }
      if (!message.SerializeToArray(bufferPtr + sizeof(packet::PacketHeader), messageSize))
      {
        return false;
      }
      packet::PacketHeader* header = reinterpret_cast<packet::PacketHeader*>(bufferPtr);
      header->messageSize = messageSize;
      header->messageType = static_cast<int16_t>(type);
      return true;
    }

    const void* GetPacketPtr() const
    {
      return bufferPtr;
    }

    const void* GetMessagePtr() const
    {
      return bufferPtr + sizeof(packet::PacketHeader);
    }

    int GetPacketByteSize() const
    {
      return static_cast<int>(sizeof(packet::PacketHeader)) + GetMessageByteSize();
    }

    int GetMessageByteSize() const
    {
      return reinterpret_cast<const packet::PacketHeader*>(bufferPtr)->messageSize;
    }

  private:

    char staticBuffer[1024]{ 0, };
    char* bufferPtr = nullptr;
    int bufferSize = 0;
  };
}
