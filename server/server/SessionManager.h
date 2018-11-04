#pragma once

#include <unordered_map>
#include <mutex>
#include "Singleton.h"
#include "UserSession.h"

class SessionManager : public Singleton<SessionManager>
{
public:
  
  friend class Singleton<SessionManager>;

  using LockGuard = std::lock_guard<std::mutex>;

  ~SessionManager();

  void RegisterSession(int64_t networkId, std::shared_ptr<UserSession> session);
  void UnregisterSession(int64_t networkId);
  std::shared_ptr<UserSession> GetSession(int64_t networkId);

private:

  SessionManager();

  std::mutex mutex;
  std::unordered_map<int64_t, std::shared_ptr<UserSession>> sessions;
};

