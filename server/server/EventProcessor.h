#pragma once

#include <queue>
#include <thread>
#include <mutex>
#include <google/protobuf/message.h>
#include "Singleton.h"

class EventProcessor : public Singleton<EventProcessor>
{
public:

  EventProcessor();
  ~EventProcessor();
  void PushEvent(const void* src, size_t size);
  void Run();
  void Stop();
  void WaitForStop();

protected:

  void Start();
  void Update();
  void ProcessEvent(const void* src, size_t size);

private:

  using LockGuard = std::lock_guard<std::mutex>;

  std::mutex queueMutex;
  std::queue<std::vector<char>> eventQueue;
  std::thread thread;
  bool shouldStop = false;
};