#include "Server.h"
#include "nta/tcp_server.hpp"
#include "UserSession.h"
#include "RelayServerEventProcessor.h"

Server::Server()
{
}


Server::~Server()
{
  RelayServerEventProcessor::GetInstance().Stop();
  RelayServerEventProcessor::GetInstance().WaitForStop();
}

void Server::Run()
{
  RelayServerEventProcessor::GetInstance().Run();

  nta::tcp_server<UserSession> server("127.0.0.1", 16333);
  server.run();
}
