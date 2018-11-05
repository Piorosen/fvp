#include "UserGroup.h"

UserGroup::UserGroup()
{
}


UserGroup::~UserGroup()
{
}

void UserGroup::AddUser(RoomUser user)
{
	users.emplace_back(std::move(user));
}

int UserGroup::GetUserCount() const
{
	return static_cast<int>(users.size());
}

RoomUser* UserGroup::GetUser(int index)
{
	auto i = users.begin();
	std::advance(i, index);
	return i != users.end() ? &(*i) : nullptr;
}

void UserGroup::RemoveUser(int index)
{
	auto i = users.begin();
	std::advance(i, index);
	users.erase(i);
}