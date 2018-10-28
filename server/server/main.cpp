
#include <boost/asio.hpp>
#include <google/protobuf/stubs/common.h>
#include "Server.h"

int main()
{
  //boost::asio::io_context context;
  //boost::asio::io_context::work work(context);
  //context.run();

  Server server;
  server.Run();

  google::protobuf::ShutdownProtobufLibrary();

  return 0;
}