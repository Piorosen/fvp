#pragma once

#include <google/protobuf/message.h>
#include <packet.pb.h>
#include "PacketHeader.h"

namespace packet
{
  class PacketReader
  {
  public:

    PacketReader()
    {
    }

    PacketReader(const void* buffer, int bufferSize) :
      bufferPtr(static_cast<const char*>(buffer)),
      bufferSize(bufferSize)
    {
    }

    PacketReader(const PacketReader& rhs)
    {
      *this = rhs;
    }

    const packet::PacketHeader* GetPacketHeader() const
    {
      return reinterpret_cast<const packet::PacketHeader*>(bufferPtr);
    }

    const void* GetSerializedMessagePtr() const
    {
      return bufferPtr + sizeof(packet::PacketHeader);
    }

    bool DeserializeMessage(google::protobuf::Message& message) const
    {
      return message.ParseFromArray(bufferPtr + sizeof(packet::PacketHeader), bufferSize - static_cast<int>(sizeof(packet::PacketHeader)));
    }

    PacketReader& operator=(const PacketReader& rhs)
    {
      bufferPtr = rhs.bufferPtr;
      bufferSize = rhs.bufferSize;
      return *this;
    }

  private:

    const char* bufferPtr = nullptr;
    int bufferSize = 0;
  };
}
