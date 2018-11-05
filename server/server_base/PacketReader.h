#pragma once

#include <google/protobuf/message.h>
#include "PacketHeader.h"

namespace packet
{
  class PacketReader
  {
  public:

		PacketReader();
		PacketReader(const void* buffer, int bufferSize);
		PacketReader(const PacketReader& rhs);
		const packet::PacketHeader* GetPacketHeader() const;
		const void* GetSerializedMessagePtr() const;
		bool DeserializeMessage(google::protobuf::Message& message) const;
		PacketReader& operator=(const PacketReader& rhs);

  private:

    const char* bufferPtr = nullptr;
    int bufferSize = 0;
  };
}
