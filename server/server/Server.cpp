#include "Server.h"
#include "nta/tcp_server.hpp"
#include "UserSession.h"
#include "EventProcessor.h"

Server::Server()
{
}


Server::~Server()
{
  EventProcessor::GetInstance().Stop();
  EventProcessor::GetInstance().WaitForStop();
}

void Server::Run()
{
	EventProcessor::GetInstance().Run();

  nta::tcp_server<UserSession> server("127.0.0.1", 16333);
  server.run();
}
