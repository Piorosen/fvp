
#include <google/protobuf/stubs/common.h>
#include "Server.h"

int main()
{
  Server server;
  server.Run();

  google::protobuf::ShutdownProtobufLibrary();

  return 0;
}