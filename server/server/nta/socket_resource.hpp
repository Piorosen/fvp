#pragma once

#include <queue>
#include <vector>
#include <boost/asio/io_context.hpp>
#include <boost/asio/io_context_strand.hpp>
#include <boost/asio/ip/tcp.hpp>
#include "object_pool.hpp"
#include "event_handler.hpp"

namespace nta
{
  struct socket_resource
  {
  public:

    socket_resource(boost::asio::io_context& context, nta::object_pool& pool) :
      strand(context),
      socket(context),
      pool(pool)
    {
    }

    socket_resource(const socket_resource&) = delete;
    socket_resource(socket_resource&&) = delete;

    boost::asio::io_context::strand strand;
    boost::asio::ip::tcp::socket socket;
    object_pool& pool;
    std::vector<char> recvBuf;
    std::queue<std::vector<char>> sendQueue;
    std::vector<std::vector<char>> sendBuffers;
	std::vector<boost::asio::const_buffer> sendBuffers2;
  };
}