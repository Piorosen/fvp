#pragma once

#include <google/protobuf/message.h>
#include <packet.pb.h>
#include "EventProcessor.h"

class RelayServerEventProcessor : public EventProcessor, public Singleton<RelayServerEventProcessor>
{
public:

  friend class Singleton<RelayServerEventProcessor>;

  void HandleConnect(int64_t networkId, const packet::Connect& message);
  void HandleDisconnect(int64_t networkId, const packet::Disconnect& message);
  void HandleLogin(int64_t networkId, const packet::LoginReq& message);
  void HandleMove(int64_t networkId, const packet::MoveReq& message);
  void Send(int64_t networkId, packet::Type type, const google::protobuf::Message& message);

protected:

  virtual void Start();
  virtual void Update();
  virtual void HandleDefaultEvent(int64_t networkId, const void* src, int size);
};