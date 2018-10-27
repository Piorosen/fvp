#pragma once

#include <google/protobuf/message.h>
#include "EventProcessor.h"

class RelayServerEventProcessor : public EventProcessor, public Singleton<RelayServerEventProcessor>
{
public:

  void HandleLogin(int64_t networkId, const packet::Login& message);

protected:

  virtual void Start();
  virtual void Update();
  virtual void HandleDefaultEvent(int64_t networkId, const void* src, int size);

private:
};