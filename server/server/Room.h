#pragma once

#include "UserGroup.h"

class Room
{
public:

	Room();
	~Room();
	void SetRoomName(const std::string& name);
	const std::string& GetRoomName() const;
	UserGroup* GetUserGroup();
	int GetMaxUserCount() const;
	void SetMaxUserCount(int count);

private:

	std::string name;
	UserGroup group;
	int maxUserCount = 0;
};