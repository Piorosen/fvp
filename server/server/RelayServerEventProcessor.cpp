#include "RelayServerEventProcessor.h"
#include <iostream>
#include <thread>

void RelayServerEventProcessor::Start()
{
  BindHandler(packet::Type::LOGIN, &RelayServerEventProcessor::HandleLogin);
}

void RelayServerEventProcessor::Update()
{
  EventProcessor::Update();
}

void RelayServerEventProcessor::HandleDefaultEvent(int64_t networkId, const void* src, int size)
{
}

void RelayServerEventProcessor::HandleLogin(int64_t networkId, const packet::Login& message)
{
  auto id = std::this_thread::get_id();
  std::cout << id << ": "<< count++ << std::endl;
}