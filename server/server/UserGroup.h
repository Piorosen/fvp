#pragma once

#include <list>
#include "RoomUser.h"

class UserGroup
{
public:
	
	UserGroup();
	~UserGroup();
	void AddUser(RoomUser user);
	int GetUserCount() const;
	RoomUser* GetUser(int index);
	void RemoveUser(int index);

private:

	std::list<RoomUser> users;
};