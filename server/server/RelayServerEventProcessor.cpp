#include "RelayServerEventProcessor.h"

void RelayServerEventProcessor::Start()
{
  BindHandler(packet::Type::LOGIN, &RelayServerEventProcessor::HandleLogin);
}

void RelayServerEventProcessor::Update()
{
}

void RelayServerEventProcessor::HandleDefaultEvent(const void* src, int size)
{
}

void RelayServerEventProcessor::HandleLogin(const packet::Login& message)
{
}