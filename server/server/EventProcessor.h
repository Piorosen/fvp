#pragma once

#include "Singleton.h"

class EventProcessor : public Singleton<EventProcessor>
{
public:

  EventProcessor();
  ~EventProcessor();
  //void PushEvent();

private:
};

