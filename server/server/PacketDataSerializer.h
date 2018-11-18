#pragma once

#include <packet_data.pb.h>
#include "Room.h"
#include "RoomUser.h"

class PacketDataSerializer
{
public:
	PacketDataSerializer();
	~PacketDataSerializer();
	bool Serialize(const Room& room, packet::Room& outRoom);
	bool Serialize(const RoomUser& user, packet::RoomUser& outUser);
};

