#include <google/protobuf/message.h>
#include <boost/asio.hpp>

int main()
{
  boost::asio::io_context ctx;
  boost::asio::ip::tcp::socket socket(ctx);

  return 0;
}