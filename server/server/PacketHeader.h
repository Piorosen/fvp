#pragma once

#include <cstdint>

#pragma pack(push, 1)

struct PacketHeader
{
  int16_t packetSize;
};

#pragma pack(pop)