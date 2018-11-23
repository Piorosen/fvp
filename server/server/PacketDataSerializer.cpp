#include "PacketDataSerializer.h"



PacketDataSerializer::PacketDataSerializer()
{
}


PacketDataSerializer::~PacketDataSerializer()
{
}

bool PacketDataSerializer::Serialize(const Room& room, packet::Room& outRoom)
{
	outRoom.Clear();
	outRoom.set_id(room.GetId());
	outRoom.set_name(room.GetRoomName());
	outRoom.set_max_user_count(room.GetMaxUserCount());
	for (int i = 0; i != room.GetUserGroup()->GetUserCount(); ++i)
	{
		auto addRoomUser = outRoom.add_room_users();
		const RoomUser* user = room.GetUserGroup()->GetUser(i);
		if (!Serialize(*user, *addRoomUser))
		{
			outRoom.Clear();
			return false;
		}
	}
	return true;
}

bool PacketDataSerializer::Serialize(const RoomUser& user, packet::RoomUser& outUser)
{
	outUser.Clear();
	outUser.set_name(user.name);
	outUser.set_network_id(user.networkId);
	*outUser.mutable_position() = user.position;
	return true;
}