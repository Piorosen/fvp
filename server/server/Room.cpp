#include "Room.h"



Room::Room()
{
}


Room::~Room()
{
}

UserGroup* Room::GetUserGroup()
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

void Room::SetMaxUserCount(int count)
{
	maxUserCount = count;
}