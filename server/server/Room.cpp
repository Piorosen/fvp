#include "Room.h"
#include "SessionManager.h"
#include "PacketReader.h"
#include <packet.pb.h>

Room::Room(int64_t roomId) :
	id(roomId)
{
}

Room::~Room()
{
}

int64_t Room::GetId() const
{
	return id;
}

void Room::EnterRoom(RoomUser roomUser)
{
	auto session = SessionManager::GetInstance().GetSession(roomUser.networkId);
	if (!session)
	{
		return;
	}

	if (started)
	{
		return;
	}

	if (maxUserCount <= this->GetUserGroup()->GetUserCount())
	{
		return;
	}

	GetUserGroup()->AddUser(std::move(roomUser));
}

UserGroup* Room::GetUserGroup()
{
  return &group;
}

const UserGroup* Room::GetUserGroup() const
{
	return &group;
}

void Room::SetRoomName(const std::string& name)
{
  this->name = name;
}

const std::string& Room::GetRoomName() const
{
  return name;
}

int Room::GetMaxUserCount() const
{
	return maxUserCount;
}

bool Room::IsFull() const
{
	return GetMaxUserCount() <= GetUserGroup()->GetUserCount();
}

void Room::SetMaxUserCount(int count)
{
	maxUserCount = count;
}

bool Room::RemoveUser(int64_t networkId)
{
	if (GetUserGroup()->FindUserByNetworkId(networkId) == nullptr)
	{
		return false;
	}
	GetUserGroup()->RemoveUser(networkId);
	return true;
}