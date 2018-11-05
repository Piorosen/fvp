#include "PacketReader.h"

namespace packet
{
	PacketReader::PacketReader()
	{
	}

	PacketReader::PacketReader(const void* buffer, int bufferSize) :
		bufferPtr(static_cast<const char*>(buffer)),
		bufferSize(bufferSize)
	{
	}

	PacketReader::PacketReader(const PacketReader& rhs)
	{
		*this = rhs;
	}

	const packet::PacketHeader* PacketReader::GetPacketHeader() const
	{
		return reinterpret_cast<const packet::PacketHeader*>(bufferPtr);
	}

	const void* PacketReader::GetSerializedMessagePtr() const
	{
		return bufferPtr + sizeof(packet::PacketHeader);
	}

	bool PacketReader::DeserializeMessage(google::protobuf::Message& message) const
	{
		return message.ParseFromArray(bufferPtr + sizeof(packet::PacketHeader), bufferSize - static_cast<int>(sizeof(packet::PacketHeader)));
	}

	PacketReader& PacketReader::operator=(const PacketReader& rhs)
	{
		bufferPtr = rhs.bufferPtr;
		bufferSize = rhs.bufferSize;
		return *this;
	}
}
