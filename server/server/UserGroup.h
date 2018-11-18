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
	const RoomUser* GetUser(int index) const;
	void RemoveUser(int index);
  RoomUser* FindUserByNetworkId(int64_t networkId);

private:

	std::list<RoomUser> users;
};