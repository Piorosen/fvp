#pragma once

#include <google/protobuf/message.h>
#include <packet.pb.h>

namespace packet
{
  class PacketBuilder
  {
  public:

		PacketBuilder();
		PacketBuilder(void* buffer, int bufferSize);
		bool BuildPacket(packet::Type type, const google::protobuf::Message& message);
		const void* GetPacketPtr() const;
		const void* GetMessagePtr() const;
		int GetPacketByteSize() const;
		int GetMessageByteSize() const;

  private:

    char staticBuffer[1024]{ 0, };
    char* bufferPtr = nullptr;
    int bufferSize = 0;
  };
}
