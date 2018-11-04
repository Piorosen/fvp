#include "SessionManager.h"



SessionManager::SessionManager()
{
}


SessionManager::~SessionManager()
{
}

void SessionManager::RegisterSession(int64_t networkId, std::shared_ptr<UserSession> session)
{
  LockGuard guard(mutex);
  sessions[networkId] = std::move(session);
}

void SessionManager::UnregisterSession(int64_t networkId)
{
  LockGuard guard(mutex);
  sessions.erase(networkId);
}

std::shared_ptr<UserSession> SessionManager::GetSession(int64_t networkId)
{
  LockGuard guard(mutex);
  auto i = sessions.find(networkId);
  return i != sessions.end() ? i->second : nullptr;
}