#include "EventProcessor.h"



EventProcessor::EventProcessor()
{
}

EventProcessor::~EventProcessor()
{
  WaitForStop();
}

void EventProcessor::PushEvent(const void* src, size_t size)
{
  std::vector<char> buffer;
  buffer.resize(size);
  memcpy(buffer.data(), src, size);

  LockGuard guard(queueMutex);
  eventQueue.push(std::move(buffer));
}

void EventProcessor::Start()
{
}

void EventProcessor::Update()
{
  int count = 0;
  {
    LockGuard guard(queueMutex);
    while (!eventQueue.empty())
    {
      eventQueue.pop();
      ++count;
    }
  }

  for (; count != 0; --count)
  {
    //DispatchEvent();
  }
}

void EventProcessor::Run()
{
  this->thread = std::thread([this]() {
    Start();
    while (!shouldStop)
    {
      Update();
    }
  });
}

void EventProcessor::Stop()
{
  shouldStop = true;
}

void EventProcessor::WaitForStop()
{
  if (this->thread.joinable())
  {
    thread.join();
  }
}

void EventProcessor::ProcessEvent(const void* src, size_t size)
{
}