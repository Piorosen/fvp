#pragma once

#include <cstdint>
#include <string>
#include <packet.pb.h>

class RoomUser
{
public:

	int64_t networkId;
	std::string name;
	packet::Vector3 position;
};