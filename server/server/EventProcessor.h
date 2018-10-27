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
  void PushEvent(const void* src, size_t size);
  void Run();
  void Stop();
  void WaitForStop();

  template < typename ProcessorType, typename MessageType >
  inline void BindHandler(packet::Type type, void(ProcessorType::*handler)(const MessageType&))
  {
    handlers[type] = [this, handler](const void* data, int size) {
      MessageType message;
      if (message.ParseFromArray(data, size))
      {
        auto self = static_cast<ProcessorType*>(this);
        (self->*handler)(message);
      }
    };
  }

protected:

  virtual void Start();
  virtual void Update();
  virtual void HandleDefaultEvent(const void* src, int size) = 0;

private:

  void DispatchEvent(const void* src, int size);

  using LockGuard = std::lock_guard<std::mutex>;

  std::mutex queueMutex;
  std::queue<std::vector<char>> eventQueue;
  std::vector<std::vector<char>> eventBuffer;
  std::thread thread;
  bool shouldStop = false;
  std::map<packet::Type, std::function<void(const void*, int)>> handlers;
  //std::chrono::duration<int32_t> timeLimit;
  //std::chrono::time_point<std::chrono::steady_clock>;
};