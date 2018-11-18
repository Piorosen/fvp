#include "Room.h"
#include "SessionManager.h"
#include "PacketReader.h"
#include <packet.pb.h>

Room::Room()
{
}

Room::~Room()
{
}

void Room::EnterRoom(int64_t networkId)
{
	auto session = SessionManager::GetInstance().GetSession(networkId);
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

	RoomUser roomUser;

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