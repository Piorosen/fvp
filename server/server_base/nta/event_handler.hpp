#pragma once

#include <boost/system/error_code.hpp>

namespace nta
{
  class event_handler
  {
  public:
    virtual void on_received(const boost::system::error_code& err, const void* data, std::size_t size) = 0;
    virtual void on_sent(const boost::system::error_code& err) = 0;
    virtual void on_connected() = 0;
    virtual void on_disconnected() = 0;
  };
}