#include "Server.h"
#include "nta/tcp_server.hpp"
#include "UserSession.h"
#include "ThreadPool.h"
#include "RelayServerEventProcessor.h"

Server::Server()
{
}


Server::~Server()
{
  RelayServerEventProcessor::GetInstance().Stop();
  ThreadPool::GetInstance().Stop();
}

void Server::Run()
{
  ThreadPool::GetInstance().Run(std::thread::hardware_concurrency());
  RelayServerEventProcessor::GetInstance().Run();

  nta::tcp_server<UserSession> server("0.0.0.0", 16333);
  server.run();
}
