#pragma once

#include <cstdint>

#pragma pack(push, 1)

namespace packet
{
  struct PacketHeader
  {
    int16_t packetSize;
  };
}

#pragma pack(pop)