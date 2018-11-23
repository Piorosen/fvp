#pragma once

#include <queue>
#include <packet.pb.h>
#include "UserGroup.h"

class Room
{
public:

	Room(int64_t roomId);
	~Room();
	int64_t GetId() const;
	void SetRoomName(const std::string& name);
	const std::string& GetRoomName() const;
	UserGroup* GetUserGroup();
	const UserGroup* GetUserGroup() const;
	int GetMaxUserCount() const;
	void SetMaxUserCount(int count);
	void EnterRoom(int64_t networkId);
	bool IsFull() const;
	bool RemoveUser(int64_t networkId);

private:

	int64_t id = 0;
	std::string name = "";
	UserGroup group;
	int maxUserCount = 0;
	int randomSeed = 0;
	std::chrono::milliseconds worldTickMs;
	bool started = false;
};