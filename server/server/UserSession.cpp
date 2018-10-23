#include "UserSession.h"
#include "EventProcessor.h"

UserSession::UserSession()
{
}


UserSession::~UserSession()
{
}

void UserSession::on_received(const boost::system::error_code & err, const void * data, std::size_t size)
{
  if (err)
  {
    return;
  }

  currentPacketSize += size;
}

void UserSession::on_sent(const boost::system::error_code & err)
{
}

void UserSession::on_connected()
{
  //EventProcessor::GetInstance().PushEvent();
}

void UserSession::on_disconnected()
{
  //EventProcessor::GetInstance().PushEvent();
}
