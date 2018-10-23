#include "Server.h"
#include "nta/tcp_server.hpp"
#include "UserSession.h"

Server::Server()
{
}


Server::~Server()
{
}

void Server::Run()
{
  nta::tcp_server<UserSession> server("127.0.0.1", 16333);
  server.run();
}
