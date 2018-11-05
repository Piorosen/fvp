#pragma once

#include <cstdint>

#pragma pack(push, 1)

namespace packet
{
  struct PacketHeader
  {
    int16_t messageSize;
    int16_t messageType;
  };
}

#pragma pack(pop)