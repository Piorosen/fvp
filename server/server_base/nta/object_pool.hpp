#pragma once

namespace nta
{
  class object_pool
  {
  public:
    virtual void* alloc() = 0;
    virtual void dealloc(void* ptr) = 0;
  };
}