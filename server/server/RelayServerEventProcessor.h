#pragma once

#include <google/protobuf/message.h>
#include "EventProcessor.h"

class RelayServerEventProcessor : public EventProcessor, public Singleton<RelayServerEventProcessor>
{
public:

  void HandleLogin(const packet::Login& message);

protected:

  virtual void Start();
  virtual void Update();
  virtual void HandleDefaultEvent(const void* src, int size);

private:
};