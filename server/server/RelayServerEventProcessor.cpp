#include "RelayServerEventProcessor.h"

void RelayServerEventProcessor::Start()
{
  BindHandler(packet::Type::LOGIN, &RelayServerEventProcessor::HandleLogin);
}

void RelayServerEventProcessor::Update()
{
}

void RelayServerEventProcessor::HandleDefaultEvent(int64_t networkId, const void* src, int size)
{
}

void RelayServerEventProcessor::HandleLogin(int64_t networkId, const packet::Login& message)
{
}