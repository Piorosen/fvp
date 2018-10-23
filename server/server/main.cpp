#include <google/protobuf/message.h>
#include <boost/asio.hpp>
#include "Server.h"
#include <packet.pb.h>

int main()
{
  packet::LoginReq req;
  req.Clear();

  Server server;
  server.Run();

  return 0;
}