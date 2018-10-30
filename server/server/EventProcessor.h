#pragma once

#include <queue>
#include <thread>
#include <mutex>
#include <chrono>

#include <packet.pb.h>
#include "Singleton.h"

class EventProcessor
{
public:

  EventProcessor();
  ~EventProcessor();
  void PushEvent(int64_t networkId, const void* src, size_t size);
  void Run();
  void Stop();

  template < typename ProcessorType, typename MessageType >
  inline void BindHandler(packet::Type type, void(ProcessorType::*handler)(int64_t, const MessageType&))
  {
    handlers[type] = [this, handler](int64_t networkId, const void* data, int size) {
      MessageType message;
      if (message.ParseFromArray(data, size))
      {
        auto self = static_cast<ProcessorType*>(this);
        (self->*handler)(networkId, message);
      }
    };
  }

protected:

  virtual void Start();
  virtual void Update();
  virtual void HandleDefaultEvent(int64_t networkId, const void* src, int size) = 0;

private:
  
  void PostUpdate();
  void DispatchEvent(int64_t networkId, const void* src, int size);

  using LockGuard = std::lock_guard<std::mutex>;

  std::mutex queueMutex;
  std::queue<std::pair<int64_t, std::vector<char>>> eventQueue;
  std::vector<std::pair<int64_t, std::vector<char>>> eventBuffer;
  bool shouldStop = false;
  std::map<packet::Type, std::function<void(int64_t, const void*, int)>> handlers;
//  std::chrono::milliseconds timeLimitMs;
};