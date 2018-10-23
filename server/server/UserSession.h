#pragma once

#include <cstdint>
#include "nta/tcp_session.hpp"

class UserSession : public nta::tcp_session
{
public:
  UserSession();
  virtual ~UserSession();

  virtual void on_received(const boost::system::error_code & err, const void * data, std::size_t size) override;
  virtual void on_sent(const boost::system::error_code & err) override;
  virtual void on_connected() override;
  virtual void on_disconnected() override;

private:
  
  int16_t packetSize;
  int32_t currentPacketSize;
  char packetBuffer[1024 * 8];
};

