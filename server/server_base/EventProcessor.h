#pragma once

#include <queue>
#include <thread>
#include <mutex>
#include <chrono>
#include <map>
#include <functional>
#include <packet_type.pb.h>
#include "Singleton.h"

struct Handler
{
	std::function<void(int64_t, const void*, int)> handler;
	std::string name;
};

class EventProcessor
{
public:

  EventProcessor();
  ~EventProcessor();
  void PushEvent(int64_t networkId, const void* src, size_t size);
  void Run();
  void Stop();

  template < typename ProcessorType, typename MessageType >
  inline void BindHandler(packet::Type type, void(ProcessorType::*handler)(int64_t, const MessageType&), const std::string& name = "")
  {
		auto& h = handlers[type];
		h.handler = [this, handler](int64_t networkId, const void* data, int size) {
      MessageType message;
      if (message.ParseFromArray(data, size))
      {
        auto self = static_cast<ProcessorType*>(this);
        (self->*handler)(networkId, message);
      }
    };
		h.name = name;
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
  std::map<packet::Type, Handler> handlers;
//  std::chrono::milliseconds timeLimitMs;
};