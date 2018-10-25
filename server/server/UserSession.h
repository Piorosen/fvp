#pragma once

#include <cstdint>
#include <google/protobuf/message.h>
#include "nta/tcp_session.hpp"

class UserSession : public nta::tcp_session
{
public:

  constexpr static int32_t MaxPacketSize = 1024 * 8;

  UserSession();
  virtual ~UserSession();

  virtual void on_received(const boost::system::error_code & err, const void * data, std::size_t size) override;
  virtual void on_sent(const boost::system::error_code & err) override;
  virtual void on_connected() override;
  virtual void on_disconnected() override;

private:

  bool SerializeToPacketBuffer(const google::protobuf::Message& message);
  
  int32_t packetSize = 0;
  int32_t currentPacketSize = 0;
  char packetBuffer[MaxPacketSize] = {0,};
  int64_t networkId = 0;
  inline static int64_t NetworkIdAllocator;
};

